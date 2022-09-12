using RestApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserSearchController : ControllerBase
    {
        private readonly IUserService service;
        public UserSearchController(IUserService userService)
        {
            service = userService;
        }

        [HttpGet("SearchUser/{word}")]
        public IActionResult SearchUser(string word)
        {
            var users = service.GetUsers().Where(x =>
                x.FirstName.ToLower().Contains(word.ToLower()) || 
                x.LastName.ToLower().Contains(word.ToLower()) ||
                x.Email.ToLower().Contains(word.ToLower()) ||
                x.LoginId.ToLower().Contains(word.ToLower())).ToList();

            return Ok(users);
        }
    }
}
