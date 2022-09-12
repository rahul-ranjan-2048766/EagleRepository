using RestApiForTweets.Models;
using RestApiForTweets.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace RestApiForTweets.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TweetController : ControllerBase
    {
        private readonly ITweetService service;
        private readonly ILogger<TweetController> logger;
        public TweetController(ITweetService tweetService, ILogger<TweetController> logger)
        {
            service = tweetService;
            this.logger = logger;
        }

        [HttpPost("Add")]
        public IActionResult Add([FromBody] Tweet tweet)
        {
            tweet.Id = null;
            tweet.DateTime = DateTime.Now;
            service.Add(tweet);

            logger.LogInformation("The tweet is successfully added to the database....");

            return Ok(new { message = "The tweet is successfully added to the database...." });
        }

        [HttpDelete("DeleteAll")]
        public IActionResult DeleteAll()
        {
            service.DeleteAll();

            logger.LogInformation("All the tweets have been deleted successfully from the database....");
            return Ok(new { message = "All the tweets have been deleted successfully from the database...." });
        }

        [HttpDelete("DeleteById/{id}")]
        public IActionResult DeleteById(string? id)
        {
            service.DeleteById(id);

            logger.LogInformation("The tweet is successfully deleted from the database successfully....");
            return Ok(new { message = "The tweet is successfully deleted from the database successfully...." });
        }

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(string? id)
        {
            logger.LogWarning("The tweet details are being retrieved from the database....");
            return Ok(service.GetById(id));
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            logger.LogWarning("All the tweet details are being retrieved from the database....");
            return Ok(service.GetAll().OrderByDescending(x => x.DateTime));
        }

        [HttpPut("Update")]
        public IActionResult Update([FromBody] Tweet tweet)
        {
            service.Update(tweet);

            logger.LogInformation("The tweet is successfully updated in the database....");
            return Ok(new { message = "The tweet is successfully updated in the database...." });
        }
    }
}
