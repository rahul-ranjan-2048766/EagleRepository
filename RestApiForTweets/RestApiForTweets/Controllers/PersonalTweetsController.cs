using RestApiForTweets.Models;
using RestApiForTweets.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace RestApiForTweets.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PersonalTweetsController : ControllerBase
    {
        private readonly ITweetService tweetService;
        private readonly ICommentService commentService;
        private readonly ILogger<PersonalTweetsController> logger;
        public PersonalTweetsController(ITweetService tweetService, ICommentService commentService, ILogger<PersonalTweetsController> logger)
        {
            this.tweetService = tweetService;
            this.commentService = commentService;
            this.logger = logger;
        }

        [HttpGet("Get/{user}")]
        public IActionResult Get(string? user)
        {
            var tweets = tweetService.GetAll().Where(x => x.Sender.ToLower().Equals(user.ToLower()));

            var comments = commentService.GetAll();

            List<TweetAndComments> list = new List<TweetAndComments>();

            foreach(var tweet in tweets)
            {
                list.Add(new TweetAndComments 
                { 
                    Sender = tweet.Sender,
                    Message = tweet.Message,
                    Tag = tweet.Tag,
                    DateTime = tweet.DateTime,
                    Id = tweet.Id,
                    Comments = comments.Where(x => x.Tweetid.Equals(tweet.Id)).ToList()
                });
            }

            logger.LogInformation("Retrieveing personal tweets along with respective comments of all the users....");

            return Ok(list);
        }
    }
}
