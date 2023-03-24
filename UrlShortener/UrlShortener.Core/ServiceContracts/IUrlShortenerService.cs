namespace UrlShortener.Core.ServiceContracts
{
    public interface IUrlShortenerService
    {
        public string ShortenUrl(string longUrl);
        public string GetLongUrlByShortUrl(string shortUrl);
    }
}
