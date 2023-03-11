using Microsoft.AspNetCore.Mvc;
using UrlShortener.Core.ServiceContracts;

namespace UrlShortenerWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UrlShortenerController : ControllerBase
    {
        private readonly ILogger<UrlShortenerController> _logger;
        private readonly IUrlShortenerService urlShortenerService;

        public UrlShortenerController(ILogger<UrlShortenerController> logger, 
            IUrlShortenerService urlShortenerService)
        {
            _logger = logger;
            this.urlShortenerService = urlShortenerService;
        }

        [HttpPost]
        public IActionResult Shorten(string longUrl)
        {
            string shortUrl = urlShortenerService.ShortenUrl(longUrl);

            return Ok(shortUrl);
        }
    }
}