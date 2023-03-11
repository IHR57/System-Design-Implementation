using Microsoft.EntityFrameworkCore;
using UrlShortener.Core;

namespace UrlShortener.Infrastructure
{
    public class UrlShortenerDbContext: DbContext
    {
        public UrlShortenerDbContext(DbContextOptions<UrlShortenerDbContext> options) : base(options)
        {

        }

        public DbSet<Url> Urls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define any relationships between models and configure database schema here
        }
    }
}
