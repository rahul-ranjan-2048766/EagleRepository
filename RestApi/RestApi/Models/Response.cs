using RestApi.Models;

namespace RestApi.Models
{
    public class Response
    {
        public string? LoginId { get; set; }
        public string? Token { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
