using RestApiForTweets.Models;
using MongoDB.Driver;

namespace RestApiForTweets.Services
{
    public class CommentService : ICommentService
    {
        private readonly string ConnectionString;
        public CommentService(IConfiguration configuration)
        {
            ConnectionString = $"mongodb://{ configuration["MongoHost"] }:{ configuration["MongoPort"] }/test";
        }
        public void Add(Comment comment)
        {
            MongoClient dbClient = new MongoClient(ConnectionString);
            dbClient.GetDatabase("test").GetCollection<Comment>("Comment").InsertOne(comment);
        }

        public void DeleteAll()
        {
            MongoClient dbClient = new MongoClient(ConnectionString);
            dbClient.GetDatabase("test").GetCollection<Comment>("Comment").DeleteMany(x => true);
        }

        public void DeleteById(string? id)
        {
            MongoClient dbClient = new MongoClient(ConnectionString);
            var filter = Builders<Comment>.Filter.Eq("Id", id);
            dbClient.GetDatabase("test").GetCollection<Comment>("Comment").DeleteOne(filter);
        }

        public List<Comment> GetAll()
        {
            MongoClient dbClient = new MongoClient(ConnectionString);
            var dbList = dbClient.GetDatabase("test").GetCollection<Comment>("Comment").AsQueryable().ToList();
            return dbList;
        }

        public Comment GetById(string? id)
        {
            MongoClient dbClient = new MongoClient(ConnectionString);
            var comment = dbClient.GetDatabase("test").GetCollection<Comment>("Comment").AsQueryable().ToList().FirstOrDefault(x => x.Id == id);
            if (comment == null) throw new SystemException();
            return comment;
        }

        public void Update(Comment comment)
        {
            MongoClient dbClient = new MongoClient(ConnectionString);
            var filter = Builders<Comment>.Filter.Eq("Id", comment.Id);
            var update = Builders<Comment>.Update
                .Set("Message", comment.Message)
                .Set("Sender", comment.Sender)
                .Set("DateTime", comment.DateTime)
                .Set("Tag", comment.Tag)
                .Set("Tweetid", comment.Tweetid);

            dbClient.GetDatabase("test").GetCollection<Comment>("Comment").UpdateOne(filter, update);
        }
    }
}
