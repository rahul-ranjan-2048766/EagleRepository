using RestApiForTweets.Models;
using MongoDB.Driver;

namespace RestApiForTweets.Services
{
    public class TweetService : ITweetService
    {
        private readonly string ConnectionString;
        public TweetService(IConfiguration configuration)
        {
            ConnectionString = $"mongodb://{ configuration["MongoHost"] }:{ configuration["MongoPort"] }/test";
        }
        public void Add(Tweet tweet)
        {
            MongoClient dbClient = new MongoClient(ConnectionString);
            dbClient.GetDatabase("test").GetCollection<Tweet>("Tweet").InsertOne(tweet);
        }

        public void DeleteAll()
        {
            MongoClient dbClient = new MongoClient(ConnectionString);
            dbClient.GetDatabase("test").GetCollection<Tweet>("Tweet").DeleteMany(x => true);
        }

        public void DeleteById(string? id)
        {
            MongoClient dbClient = new MongoClient(ConnectionString);
            var filter = Builders<Tweet>.Filter.Eq("Id", id);
            dbClient.GetDatabase("test").GetCollection<Tweet>("Tweet").DeleteOne(filter);
        }

        public List<Tweet> GetAll()
        {
            MongoClient dbClient = new MongoClient(ConnectionString);
            var dbList = dbClient.GetDatabase("test").GetCollection<Tweet>("Tweet").AsQueryable().ToList();
            return dbList;
        }

        public Tweet GetById(string? id)
        {
            MongoClient dbClient = new MongoClient(ConnectionString);
            var tweet = dbClient.GetDatabase("test").GetCollection<Tweet>("Tweet").AsQueryable().ToList().FirstOrDefault(x => x.Id == id);
            if (tweet == null) throw new SystemException();
            return tweet;
        }

        public void Update(Tweet tweet)
        {
            MongoClient dbClient = new MongoClient(ConnectionString);
            var filter = Builders<Tweet>.Filter.Eq("Id", tweet.Id);
            var update = Builders<Tweet>.Update
                .Set("Message", tweet.Message)
                .Set("Sender", tweet.Sender)
                .Set("Tag", tweet.Tag)
                .Set("DateTime", tweet.DateTime);

            dbClient.GetDatabase("test").GetCollection<Tweet>("Tweet").UpdateOne(filter, update);
        }
    }
}
