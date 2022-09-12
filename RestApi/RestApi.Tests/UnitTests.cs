using Xunit;
using RestApi.Controllers;
using RestApi.Models;
using RestApi.Helpers;
using RestApi.Services;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace RestApi.Tests
{
    public class UnitTests
    {
        /* Global Variables */

        public List<User> UsersList = new List<User>
            {
                new User
                {
                    ContactNumber = "4646464682", DateTime = DateTime.Now, Email = "email4@gmail.com", FirstName = "Rahul",
                    Id = "88226644hhllaaxx", LastName = "Rahul", LoginId = "wizard", Password = "IndiaUSA46@$"
                },

                new User
                {
                    ContactNumber = "6464646428", DateTime = DateTime.Now, Email = "email4@gmail.com", FirstName = "Sam",
                    Id = "12094686qwrtasgf", LastName = "Kumar", LoginId = "Sam", Password = "IndiaUSA46@$"
                },
                new User
                {
                    ContactNumber = "2468246824", DateTime = DateTime.Now, Email = "email2@gmail.com", FirstName = "Max",
                    Id = "65780988nhmjlopi", LastName = "Tennyson", LoginId = "max", Password = "IndiaUSA46@$"
                },
                new User
                {
                    ContactNumber = "1234567890", DateTime = DateTime.Now, Email = "email8@gmail.com", FirstName = "Robin",
                    Id = "67893214edrfbgnh", LastName = "Hood", LoginId = "Robin", Password = "IndiaUSA46@$"
                },
                new User
                {
                    ContactNumber = "2345644448", DateTime = DateTime.Now, Email = "email46@outlook.com", FirstName = "Ben",
                    Id = "45678902asdfghjk", LastName = "Tennyson", LoginId = "Ben", Password = "IndiaUSA46@$"
                },
                new User
                {
                    ContactNumber = "8888666642", DateTime = DateTime.Now, Email = "email82@outlook.com", FirstName = "Tony",
                    Id = "12345678qwertyui", LastName = "Stark", LoginId = "Tony", Password = "IndiaUSA46@$"
                }
            };


        /* Unit Tests - Service Layer */

        [Fact]
        public void PositiveTest_GetUsers()
        {
            Mock<IConfiguration> configuration = new Mock<IConfiguration>();
            configuration.Setup(x => x["ConnectionStrings:DefaultConnection"]).Returns("mongodb://localhost:27017/test");

            Mock<IMongoDatabase> mockDatabase = new Mock<IMongoDatabase>();

            Mock<IMongoClient> dbClient = new Mock<IMongoClient>();
            dbClient.Setup(x => x.GetDatabase("test", null)).Returns(mockDatabase.Object);

            var result = dbClient.Object.GetDatabase("test").GetCollection<User>("User");

            Assert.Null(result);
        }


        /* Unit Tests - Profile Controller */
        [Theory]
        [InlineData("wizard")]
        [InlineData("Sam")]
        public void PositiveTest_GetUser(string loginId)
        {           
            Mock<IUserService> service = new Mock<IUserService>();
            service.Setup(x => x.GetUsers()).Returns(UsersList);

            ProfileController controller = new ProfileController(service.Object);

            var result = controller.GetUser(loginId);
            var data = result.Should().BeOfType<OkObjectResult>().Subject;

            var actual = data.Value.Should().BeAssignableTo<User>().Subject;
            var expected = UsersList.FirstOrDefault(x => x.LoginId == loginId);

            Assert.Equal(actual.Id, expected.Id);
        }


        /* Unit Tests - UserSearch Controller */
        [Theory]
        [InlineData("wizard")]
        [InlineData("Sam")]
        public void PositiveTest_SearchUser(string word)
        {
            Mock<IUserService> service = new Mock<IUserService>();
            service.Setup(x => x.GetUsers()).Returns(UsersList);

            UserSearchController controller = new UserSearchController(service.Object);

            var result = controller.SearchUser(word);

            Assert.IsType<OkObjectResult>(result);
        }


        /* Unit Tests - User Controller */

        [Fact]
        public void PositiveTest_Get()
        {
            Mock<IUserService> service = new Mock<IUserService>();
            service.Setup(x => x.GetUsers()).Returns(UsersList);

            Mock<ILogger<UserController>> logger = new Mock<ILogger<UserController>>();

            UserController controller = new UserController(service.Object, logger.Object);
            var result = controller.Get();

            Assert.IsType<OkObjectResult>(result);
        }


        /* Unit Tests - Authenticate Controller */

        [Fact]
        public void PositiveTest_Authenticate()
        {
            Mock<IUserService> service = new Mock<IUserService>();
            service.Setup(x => x.GetUsers()).Returns(UsersList);

            Mock<IConfiguration> configuration = new Mock<IConfiguration>();
            configuration.Setup(x => x["SecretKey"]).Returns("Secret Key - The Statue Of Liberty");

            IJwtAuthenticateManager manager = new JwtAuthenticateManager(service.Object, configuration.Object);

            AuthenticateController controller = new AuthenticateController(manager);
            var result = controller.Authenticate(new UserCred 
            { 
                LoginId = "wizard",
                Password = "IndiaUSA46@$"
            });

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void NegativeTest_Authenticate()
        {
            Mock<IUserService> service = new Mock<IUserService>();
            service.Setup(x => x.GetUsers()).Returns(UsersList);

            Mock<IConfiguration> configuration = new Mock<IConfiguration>();
            configuration.Setup(x => x["SecretKey"]).Returns("Secret Key - The Statue Of Liberty");

            IJwtAuthenticateManager manager = new JwtAuthenticateManager(service.Object, configuration.Object);

            AuthenticateController controller = new AuthenticateController(manager);
            var result = controller.Authenticate(new UserCred 
            { 
                LoginId = "jwhycvhjwvcjh",
                Password = "IndiaUSA46@$jhvejhejhvjhwvjhv"
            });

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}