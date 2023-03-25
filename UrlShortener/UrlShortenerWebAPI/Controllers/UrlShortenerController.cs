using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using UrlShortener.Core.ServiceContracts;

namespace UrlShortenerWebAPI.Controllers
{
    [ApiController]
    public class UrlShortenerController : ControllerBase
    {
        private readonly IUrlShortenerService urlShortenerService;
        private readonly IDatabase _redis;

        public UrlShortenerController(IConnectionMultiplexer multiplexer, 
            IUrlShortenerService urlShortenerService)
        {
            this._redis = multiplexer.GetDatabase();
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
        public async Task<IActionResult> RedirectToUrl(string shortUrl)
        {
            string longUrl = await _redis.StringGetAsync(shortUrl);

            if(string.IsNullOrEmpty(longUrl))
            {
                longUrl = urlShortenerService.GetLongUrlByShortUrl(shortUrl);
                var setTask = _redis.StringSetAsync(shortUrl, longUrl);
                var expireTask = _redis.KeyExpireAsync(shortUrl, TimeSpan.FromSeconds(3600));

                await Task.WhenAll(setTask, expireTask);
            }

            return Redirect(longUrl);
        }
    }
}