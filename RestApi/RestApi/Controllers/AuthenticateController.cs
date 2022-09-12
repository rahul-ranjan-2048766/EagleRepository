using RestApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using RestApi.Models;

namespace RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IJwtAuthenticateManager manager;
        public AuthenticateController(IJwtAuthenticateManager jwtAuthenticateManager)
        {
            manager = jwtAuthenticateManager;
        }

        [HttpPost("Authenticate")]
        public IActionResult Authenticate([FromBody] UserCred userCred)
        {
            var response = manager.Authenticate(userCred);

            if (response == null) 
                return BadRequest(new { message = "The user does not exist or the credentials are incorrect...." });

            return Ok(response);
        }
    }
}
