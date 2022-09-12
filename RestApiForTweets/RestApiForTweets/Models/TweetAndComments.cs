namespace RestApiForTweets.Models
{
    public class TweetAndComments
    {
        public string? Id { get; set; }
        public string? Sender { get; set; }
        public string? Message { get; set; }
        public string? Tag { get; set; }
        public DateTime DateTime { get; set; }
        public List<Comment>? Comments { get; set; }
    }
}
