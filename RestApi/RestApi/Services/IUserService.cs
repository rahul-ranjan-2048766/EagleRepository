using RestApi.Models;

namespace RestApi.Services
{
    public interface IUserService
    {
        public void Add(User user);
        public void DeleteAll();
        public void DeleteById(string id);
        public User GetById(string id);
        public List<User> GetUsers();
        public void Update(User user);
    }
}
