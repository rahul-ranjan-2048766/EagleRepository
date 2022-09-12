using RestApi.Models;

namespace RestApi.Helpers
{
    public interface IJwtAuthenticateManager
    {
        public Response? Authenticate(UserCred userCred);
    }
}
