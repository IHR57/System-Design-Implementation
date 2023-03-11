using Microsoft.EntityFrameworkCore;
using UrlShortener.Core.Domain.RepositoryContracts;
using UrlShortener.Core.ServiceContracts;
using UrlShortener.Core.Services;
using UrlShortener.Infrastructure;
using UrlShortener.Infrastructure.Repositories;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // connectionString="Data Source=LocalDB;Initial Catalog=url_shortener;Integrated Security=False;User Id=iqbal;Password=155004;MultipleActiveResultSets=True"
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var blder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        IConfiguration configuration = blder.Build();

        builder.Services.AddDbContext<UrlShortenerDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ConnectionString")));

        builder.Services.AddScoped<IUniqueIdGenerator, SnowflakeIdGenerator>();
        builder.Services.AddScoped<IUrlRepository, UrlRepository>();
        builder.Services.AddScoped<IUrlShortenerService, UrlShortenerService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}