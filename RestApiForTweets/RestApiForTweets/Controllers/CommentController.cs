using RestApiForTweets.Models;
using RestApiForTweets.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace RestApiForTweets.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService service;
        private readonly ILogger<CommentController> logger;
        public CommentController(ICommentService commentService, ILogger<CommentController> logger)
        {
            service = commentService;
            this.logger = logger;
        }

        [HttpPost("Add")]
        public IActionResult Add([FromBody] Comment comment)
        {
            comment.Id = null;
            comment.DateTime = DateTime.Now;

            service.Add(comment);

            logger.LogInformation("The comment is saved into the database successfully....");

            return Ok(new { message = "The comment is saved into the database successfully...." });
        }

        [HttpDelete("DeleteAll")]
        public IActionResult DeleteAll()
        {
            service.DeleteAll();

            logger.LogInformation("All the comments are successfully deleted from the database....");
            return Ok(new { message = "All the comments are successfully deleted from the database...." });
        }

        [HttpDelete("DeleteById/{id}")]
        public IActionResult DeleteById(string? id)
        {
            service.DeleteById(id);

            logger.LogInformation("The comment is successfully deleted from the database....");
            return Ok(new { message = "The comment is successfully deleted from the database...." });
        }

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(string? id)
        {
            logger.LogWarning("Retrieveing the comment from the database....");
            return Ok(service.GetById(id));
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            logger.LogWarning("Retrieving all the comments from the database....");
            return Ok(service.GetAll());
        }

        [HttpPut("Update")]
        public IActionResult Update([FromBody] Comment comment)
        {
            service.Update(comment);

            logger.LogInformation("The comment is successfully updated in the database....");
            return Ok(new { message = "The comment is successfully updated in the database...." });
        }
    }
}
