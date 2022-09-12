using RestApiForTweets.Models;

namespace RestApiForTweets.Services
{
    public interface ICommentService
    {
        public void Add(Comment comment);
        public void DeleteAll();
        public void DeleteById(string? id);
        public Comment GetById(string? id);
        public List<Comment> GetAll();
        public void Update(Comment comment);
    }
}
