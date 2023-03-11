namespace UrlShortener.Core.Domain.RepositoryContracts
{
    public interface IUrlRepository
    {
        IEnumerable<Url> GetUrls();
        Url GetLongUrlByShortUrl(string shortUrl);
        Url GetShortUrlByLongUrl(string longUrl);
        bool IsLongUrlExists(string longUrl);
        void InsertUrl(Url url);
        void Save();
    }
}
