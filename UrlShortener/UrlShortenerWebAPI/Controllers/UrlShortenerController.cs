using Microsoft.AspNetCore.Mvc;
using UrlShortener.Core.ServiceContracts;

namespace UrlShortenerWebAPI.Controllers
{
    [ApiController]
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

        [Route("[controller]/[action]")]
        [HttpPost]
        public IActionResult Shorten(string longUrl)
        {
            string shortUrl = urlShortenerService.ShortenUrl(longUrl);

            return Ok(shortUrl);
        }

        [Route("{shortUrl}")]
        [HttpGet]
        public IActionResult RedirectToUrl(string shortUrl)
        {
            string longUrl = urlShortenerService.GetLongUrlByShortUrl(shortUrl);

            return Redirect(longUrl);
        }
    }
}