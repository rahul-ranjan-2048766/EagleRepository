using RestApi.Models;
using MongoDB.Driver;

namespace RestApi.Services
{
    public class UserService : IUserService
    {
        private readonly string ConnectionString;
        public UserService(IConfiguration configuration)
        {
            ConnectionString = $"mongodb://{ configuration["MongoHost"] }:{ configuration["MongoPort"] }/test";
        }
        public void Add(User user)
        {
            MongoClient dbClient = new MongoClient(ConnectionString);
            dbClient.GetDatabase("test").GetCollection<User>("User").InsertOne(user);
        }

        public void DeleteAll()
        {
            MongoClient dbClient = new MongoClient(ConnectionString);
            dbClient.GetDatabase("test").GetCollection<User>("User").DeleteMany(x => true);
        }

        public void DeleteById(string id)
        {
            MongoClient dbClient = new MongoClient(ConnectionString);
            var filter = Builders<User>.Filter.Eq("Id", id);
            dbClient.GetDatabase("test").GetCollection<User>("User").DeleteOne(filter);
        }

        public User GetById(string id)
        {
            MongoClient dbClient = new MongoClient(ConnectionString);
            var user = dbClient.GetDatabase("test").GetCollection<User>("User").AsQueryable().ToList().FirstOrDefault(x => x.Id == id);
            if (user == null) throw new SystemException();
            return user;
        }

        public List<User> GetUsers()
        {
            MongoClient dbClient = new MongoClient(ConnectionString);
            var dbList = dbClient.GetDatabase("test").GetCollection<User>("User").AsQueryable().ToList();
            return dbList;
        }

        public void Update(User user)
        {
            MongoClient dbClient = new MongoClient(ConnectionString);
            var filter = Builders<User>.Filter.Eq("Id", user.Id);
            var update = Builders<User>.Update.
                Set("FirstName", user.FirstName).
                Set("LastName", user.LastName).
                Set("Email", user.Email).
                Set("LoginId", user.LoginId).
                Set("Password", user.Password).
                Set("ContactNumber", user.ContactNumber);

            dbClient.GetDatabase("test").GetCollection<User>("User").UpdateOne(filter, update);
        }
    }
}
