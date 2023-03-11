namespace UrlShortener.Core.ServiceContracts
{
    public interface IUrlShortenerService
    {
        public string ShortenUrl(string longUrl);
    }
}
