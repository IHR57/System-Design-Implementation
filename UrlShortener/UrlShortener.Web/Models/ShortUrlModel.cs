using System.ComponentModel.DataAnnotations;

namespace ShortUrl.Models
{
    public class ShortUrlModel
    {
        [Required]
        public string LongUrl { get; set; }
    }
}