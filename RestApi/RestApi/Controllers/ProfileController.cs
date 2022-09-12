using RestApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IUserService service;
        public ProfileController(IUserService userService)
        {
            service = userService;
        }

        [HttpGet("GetUser/{loginId}")]
        public IActionResult GetUser(string loginId)
        {
            var user = service.GetUsers().FirstOrDefault(x => x.LoginId.Equals(loginId));
            return Ok(user);
        }
    }
}
