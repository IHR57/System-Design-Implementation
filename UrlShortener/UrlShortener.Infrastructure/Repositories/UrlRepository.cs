
using UrlShortener.Core;
using UrlShortener.Core.Domain.RepositoryContracts;

namespace UrlShortener.Infrastructure.Repositories
{
    public class UrlRepository : IUrlRepository, IDisposable
    {
        private UrlShortenerDbContext dbContext;

        public UrlRepository(UrlShortenerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Url GetLongUrlByShortUrl(string shortUrl)
        {
            return dbContext.Urls.First(url => url.ShortUrl == shortUrl);
        }

        public IEnumerable<Url> GetUrls()
        {
            return dbContext.Urls.ToList();
        }

        public bool IsLongUrlExists(string longUrl)
        {
            return dbContext.Urls.Count(url => url.LongUrl == longUrl) == 1;
        }

        public void InsertUrl(Url url)
        {
            dbContext.Urls.Add(url);
        }

        public Url GetShortUrlByLongUrl(string longUrl)
        {
            return dbContext.Urls.First(url => url.LongUrl == longUrl);
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    dbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
