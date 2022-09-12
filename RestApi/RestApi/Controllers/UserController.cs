using Microsoft.AspNetCore.Mvc;
using RestApi.Models;
using RestApi.Services;
using Microsoft.AspNetCore.Authorization;

namespace RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService service;
        private ILogger<UserController> logger;
        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            service = userService;
            this.logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("Add")]
        public IActionResult Add([FromBody] User user)
        {
            var sameLoginIdCount = service.GetUsers().Where(x => x.LoginId.Equals(user.LoginId)).ToList().Count();
            if (sameLoginIdCount > 0)
                return BadRequest(new { message = "The user with this login id already exists. Please enter a different login id...." });

            var sameUserEmailCount = service.GetUsers().Where(x => x.Email.Equals(user.Email)).ToList().Count();
            if (sameUserEmailCount > 0)
                return BadRequest(new { message = "The user with this email already exists. Please enter a different email id...." });

            user.Id = null;
            user.DateTime = DateTime.Now;
            service.Add(user);

            logger.LogInformation("The user is registered in mongodb successfully....");

            return Ok(new { message = "The user is registered in mongodb successfully...." });
        }

        [HttpDelete("DeleteAll")]
        public IActionResult DeleteAll()
        {
            service.DeleteAll();

            logger.LogInformation("All the users are deleted successfully....");
            return Ok(new { message = "All the users are deleted successfully...." });
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(string id)
        {
            service.DeleteById(id);

            logger.LogInformation("The user is deleted successfully from the database....");
            return Ok(new { message = "The user is deleted successfully from the database...." });
        }

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(string id)
        {
            logger.LogWarning("The data is being retrieved from the Azure Cosmos DB....");
            return Ok(service.GetById(id));
        }

        [HttpGet("Get")]
        public IActionResult Get()
        {
            logger.LogWarning("The list of users is being retrieved from the Azure Cosmos DB....");
            return Ok(service.GetUsers());
        }

        [HttpPut("Update")]
        public IActionResult Update([FromBody] User user)
        {
            var sameEmailIdCount = service.GetUsers().Where(x => x.Email.Equals(user.Email)).Count();

            if (sameEmailIdCount > 0)
            {
                if (sameEmailIdCount == 1)
                {
                    var temp = service.GetUsers().FirstOrDefault(x => x.Email.Equals(user.Email));
                    if (temp.Id != user.Id)
                        return BadRequest(new { message = "This email is already taken by a user. Please enter another email...." });
                    else
                    {
                        service.Update(user);

                        logger.LogInformation("The user details are updated successfully....");
                        return Ok(new { message = "The user details are updated successfully...." });
                    }
                } 
                
                return BadRequest(new { message = "This email is already taken by a user. Please enter another email...." });
            }                

            var sameLoginIdCount = service.GetUsers().Where(x => x.LoginId.Equals(user.LoginId)).Count();


            if (sameLoginIdCount > 0)
            {
                if (sameLoginIdCount != 1)
                {
                    return BadRequest(new { message = "This login id is already taken. Please enter another unique login id...." });
                }
                else
                {
                    service.Update(user);

                    logger.LogInformation("The user details are updated successfully....");
                    return Ok(new { message = "The user details are updated successfully...." });
                }
                return BadRequest(new { message = "This login id is already taken. Please enter another unique login id...." });
            }
                

            service.Update(user);

            logger.LogInformation("The user details are updated successfully....");
            return Ok(new { message = "The user details are updated successfully...." });
        }
    }
}
