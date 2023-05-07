using Microsoft.AspNetCore.Mvc;
using ShortUrl.Models;
using System.Diagnostics;
using UrlShortener.Core.ServiceContracts;
using StackExchange.Redis;
using IDatabase = StackExchange.Redis.IDatabase;

namespace ShortUrl.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDatabase _redis;
        private readonly IUrlShortenerService urlShortenerService;

        public HomeController(ILogger<HomeController> logger, IUrlShortenerService urlShortenerService, IConnectionMultiplexer multiplexer)
        {
            _logger = logger;
            this.urlShortenerService = urlShortenerService;
            this._redis = multiplexer.GetDatabase();
        }

        public IActionResult Index()
        {
            ViewData["Title"] = "Url Shortener";
            ViewData["ShortUrl"] = null;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ShortUrl([Bind("LongUrl")] ShortUrlModel model)
        {
            if (ModelState.IsValid)
            {
                string shortUrl = urlShortenerService.ShortenUrl(model.LongUrl);

                ViewData["ShortUrl"] = shortUrl;
            }

            return View("Index");
        }

        [Route("{shortUrl}")]
        [HttpGet]
        public async Task<IActionResult> RedirectToUrl(string shortUrl)
        {
            string longUrl = await _redis.StringGetAsync(shortUrl);

            if (string.IsNullOrEmpty(longUrl))
            {
                longUrl = urlShortenerService.GetLongUrlByShortUrl(shortUrl);
                var setTask = _redis.StringSetAsync(shortUrl, longUrl);
                var expireTask = _redis.KeyExpireAsync(shortUrl, TimeSpan.FromSeconds(3600));

                await Task.WhenAll(setTask, expireTask);
            }

            return Redirect(longUrl);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}