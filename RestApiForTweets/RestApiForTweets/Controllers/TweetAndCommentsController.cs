using RestApiForTweets.Models;
using RestApiForTweets.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace RestApiForTweets.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TweetAndCommentsController : ControllerBase
    {
        private readonly ITweetService tweetService;
        private readonly ICommentService commentService;
        private readonly ILogger<TweetAndCommentsController> logger;
        public TweetAndCommentsController(ITweetService tweetService, ICommentService commentService, ILogger<TweetAndCommentsController> logger)
        {
            this.tweetService = tweetService;
            this.commentService = commentService;
            this.logger = logger;
        }

        [HttpGet("GetTweetsAndComments")]
        public IActionResult GetTweetsAndComments()
        {
            var tweets = tweetService.GetAll().OrderByDescending(x => x.DateTime);

            var comments = commentService.GetAll();

            List<TweetAndComments> list = new List<TweetAndComments>();

            foreach(var tweet in tweets)
            {
                list.Add(new TweetAndComments 
                { 
                    Id = tweet.Id,
                    Sender = tweet.Sender,
                    Message = tweet.Message,
                    Tag = tweet.Tag,
                    DateTime = tweet.DateTime,
                    Comments = comments.Where(x => x.Tweetid == tweet.Id).ToList()
                });
            }

            return Ok(list);
        }
    }
}
