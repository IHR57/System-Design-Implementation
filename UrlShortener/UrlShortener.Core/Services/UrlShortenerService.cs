using System.Text;
using UrlShortener.Core.Domain.RepositoryContracts;
using UrlShortener.Core.ServiceContracts;

namespace UrlShortener.Core.Services
{
    public class UrlShortenerService : IUrlShortenerService
    {
        private readonly IUrlRepository urlRepository;
        private readonly IUniqueIdGenerator uniqueIdGenerator;

        public UrlShortenerService(IUrlRepository urlRepository, 
            IUniqueIdGenerator uniqueIdGenerator)
        {
            this.urlRepository = urlRepository;
            this.uniqueIdGenerator = uniqueIdGenerator;
        }

        public string ShortenUrl(string longUrl)
        {
            bool longUrlExists = urlRepository.IsLongUrlExists(longUrl);

            if(!longUrlExists)
            {
                long uniqueId = uniqueIdGenerator.NextId();
                string shortUrl = ToBase62(uniqueId);

                urlRepository.InsertUrl(new Url { LongUrl = longUrl, ShortUrl = shortUrl });
                urlRepository.Save();
            }

            return urlRepository.GetShortUrlByLongUrl(longUrl).ShortUrl;
        }

        public string GetLongUrlByShortUrl(string shortUrl)
        {
            return urlRepository.GetLongUrlByShortUrl(shortUrl).LongUrl;
        }

        public static string ToBase62(long value)
        {
            const string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var result = new StringBuilder();

            while(value > 0)
            {
                result.Insert(0, chars[(int)(value % 62)]);
                value /= 62;
            }

            return result.ToString();
        }

    }
}
