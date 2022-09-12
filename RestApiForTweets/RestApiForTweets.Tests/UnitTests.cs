using Xunit;
using RestApiForTweets.Controllers;
using RestApiForTweets.Models;
using RestApiForTweets.Services;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace RestApiForTweets.Tests
{
    public class UnitTests
    {

        /* Global Variables */

        public List<Tweet> TweetsList = new List<Tweet>
        { 
            new Tweet { DateTime = DateTime.Now, Id = "24688642qwerfsda", Message = "Message1", Sender = "Sender1", Tag = "Tag1" },
            new Tweet { DateTime = DateTime.Now, Id = "98766789anhsgzxf", Message = "Message2", Sender = "Sender2", Tag = "Tag2" },
            new Tweet { DateTime = DateTime.Now, Id = "46468282usaindia", Message = "Message3", Sender = "Sender3", Tag = "Tag3" },
            new Tweet { DateTime = DateTime.Now, Id = "89890706wxwxzgzg", Message = "Message4", Sender = "Sender4", Tag = "Tag4" },
        };

        public List<Comment> CommentsList = new List<Comment> 
        { 
            new Comment { DateTime = DateTime.Now, Id = "24242424tytyuaua", Message = "Message1", Sender = "Sender1", Tag = "Tag1", Tweetid = "98766789anhsgzxf" },
            new Comment { DateTime = DateTime.Now, Id = "48486262swswajhr", Message = "Message2", Sender = "Sender2", Tag = "Tag2", Tweetid = "24688642qwerfsda" },
            new Comment { DateTime = DateTime.Now, Id = "24684680wrsgrudj", Message = "Message3", Sender = "Sender3", Tag = "Tag3", Tweetid = "46468282usaindia" },
            new Comment { DateTime = DateTime.Now, Id = "12345678qwerasdf", Message = "Message4", Sender = "Sender4", Tag = "Tag4", Tweetid = "89890706wxwxzgzg" }
        };


        /* Unit Tests - Comment Service */

        [Fact]
        public void PositiveTest_GetComments()
        {
            Mock<IConfiguration> configuration = new Mock<IConfiguration>();
            configuration.Setup(x => x["ConnectionStrings:MongoDbConnectionString"]).Returns("mongodb://localhost:27017/test");

            Mock<IMongoDatabase> database = new Mock<IMongoDatabase>();

            Mock<IMongoClient> dbClient = new Mock<IMongoClient>();
            dbClient.Setup(x => x.GetDatabase("test", null)).Returns(database.Object);

            var result = dbClient.Object.GetDatabase("test", null).GetCollection<Comment>("Comment");

            Assert.Null(result);
        }


        /* Unit Tests - Tweet Service */

        [Fact]
        public void PositiveTest_GetTweets()
        {
            Mock<IConfiguration> configuration = new Mock<IConfiguration>();
            configuration.Setup(x => x["ConnectionStrings:MongoDbConnectionString"]).Returns("mongodb://localhost:27017/test");

            Mock<IMongoDatabase> database = new Mock<IMongoDatabase>();

            Mock<IMongoClient> dbClient = new Mock<IMongoClient>();
            dbClient.Setup(x => x.GetDatabase("test", null)).Returns(database.Object);

            var result = dbClient.Object.GetDatabase("test", null).GetCollection<Comment>("Comment");

            Assert.Null(result);
        }


        /* Unit Tests - Comment Controller */

        [Fact]
        public void PositiveTest_GetAllComments()
        {
            Mock<ICommentService> commentService = new Mock<ICommentService>();
            commentService.Setup(x => x.GetAll()).Returns(CommentsList);

            Mock<ILogger<CommentController>> logger = new Mock<ILogger<CommentController>>();

            CommentController controller = new CommentController(commentService.Object, logger.Object);
            var result = controller.GetAll();

            Assert.IsType<OkObjectResult>(result);
        }


        /* Unit Tests = Tweet Controller */

        [Fact]
        public void PositiveTest_GetAllTweets()
        {
            Mock<ITweetService> tweetService = new Mock<ITweetService>();
            tweetService.Setup(x => x.GetAll()).Returns(TweetsList);

            Mock<ILogger<TweetController>> logger = new Mock<ILogger<TweetController>>();

            TweetController controller = new TweetController(tweetService.Object, logger.Object);
            var result = controller.GetAll();

            Assert.IsType<OkObjectResult>(result);
        }


        /* Unit Tests - Personal Tweets */

        [Theory]
        [InlineData("Sender2")]
        [InlineData("Sender4")]
        public void GetPersonalTweetsTest(string? user)
        {
            Mock<ITweetService> tweetService = new Mock<ITweetService>();
            tweetService.Setup(x => x.GetAll()).Returns(TweetsList);

            Mock<ICommentService> commentService = new Mock<ICommentService>();
            commentService.Setup(x => x.GetAll()).Returns(CommentsList);

            Mock<ILogger<PersonalTweetsController>> logger = new Mock<ILogger<PersonalTweetsController>>();

            PersonalTweetsController controller = new PersonalTweetsController(tweetService.Object, commentService.Object, logger.Object);
            var result = controller.Get(user);

            Assert.IsType<OkObjectResult>(result);
        }


        /* Unit Tests = Tweets And Comments Controller */

        [Fact]
        public void GetTweetsAndComments()
        {
            Mock<ITweetService> tweetService = new Mock<ITweetService>();
            tweetService.Setup(x => x.GetAll()).Returns(TweetsList);

            Mock<ICommentService> commentService = new Mock<ICommentService>();
            commentService.Setup(x => x.GetAll()).Returns(CommentsList);

            Mock<ILogger<TweetAndCommentsController>> logger = new Mock<ILogger<TweetAndCommentsController>>();

            TweetAndCommentsController controller = new TweetAndCommentsController(tweetService.Object, commentService.Object, logger.Object);
            var result = controller.GetTweetsAndComments();

            Assert.IsType<OkObjectResult>(result);
        }
    }
}