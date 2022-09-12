using RestApiForTweets.Models;

namespace RestApiForTweets.Services
{
    public interface ITweetService
    {
        public void Add(Tweet tweet);
        public void DeleteAll();
        public void DeleteById(string? id);
        public Tweet GetById(string? id);
        public List<Tweet> GetAll();
        public void Update(Tweet tweet);
    }
}
