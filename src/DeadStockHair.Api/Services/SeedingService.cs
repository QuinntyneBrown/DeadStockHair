using DeadStockHair.Api.Data;
using DeadStockHair.Api.Models;
using DeadStockHair.Cli.Services;
using Microsoft.EntityFrameworkCore;

namespace DeadStockHair.Api.Services;

public class SeedingService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IRetailerScraperService _scraperService;
    private readonly ILogger<SeedingService> _logger;

    public SeedingService(
        IServiceProvider serviceProvider,
        IRetailerScraperService scraperService,
        ILogger<SeedingService> logger)
    {
        _serviceProvider = serviceProvider;
        _scraperService = scraperService;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (await context.Retailers.AnyAsync())
        {
            _logger.LogInformation("Database already contains retailers, skipping seed");
            return;
        }

        _logger.LogInformation("Database is empty, attempting to seed via CLI scraper...");

        try
        {
            var scrapedRetailers = await _scraperService.ScanAsync(headless: true);
            var scraped = scrapedRetailers.ToList();

            if (scraped.Count > 0)
            {
                _logger.LogInformation("CLI scraper found {Count} retailer(s), seeding database", scraped.Count);

                var entities = scraped.Select(r => new Retailer
                {
                    Name = r.Name,
                    Url = ExtractDomain(r.Url),
                    Status = RetailerStatus.Unknown,
                    DiscoveredAt = DateTime.UtcNow,
                }).ToList();

                await context.Retailers.AddRangeAsync(entities);
                await context.SaveChangesAsync();

                _logger.LogInformation("Successfully seeded {Count} retailers from CLI scraper", entities.Count);
                return;
            }

            _logger.LogWarning("CLI scraper returned no results, falling back to static seed data");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "CLI scraper failed, falling back to static seed data");
        }

        await SeedStaticDataAsync(context);
    }

    private async Task SeedStaticDataAsync(ApplicationDbContext context)
    {
        _logger.LogInformation("Seeding static fallback data...");

        var retailers = new[]
        {
            new Retailer { Name = "DROP DEAD Extensions", Url = "dropdeadextensions.com", Status = RetailerStatus.InStock },
            new Retailer { Name = "Hair Overstock", Url = "hairoverstock.com", Status = RetailerStatus.InStock },
            new Retailer { Name = "Viola Hair Extensions", Url = "violahairextensions.co", Status = RetailerStatus.InStock },
            new Retailer { Name = "ParaHair Canada", Url = "parahair.ca", Status = RetailerStatus.InStock },
            new Retailer { Name = "Golden Lush Extensions", Url = "goldenlushextensions.com", Status = RetailerStatus.InStock },
            new Retailer { Name = "Chiquel Hair", Url = "chiquel.ca", Status = RetailerStatus.InStock, DiscoveredAt = DateTime.UtcNow.AddDays(-2) },
            new Retailer { Name = "Luxe Strand Co", Url = "luxestrand.com", Status = RetailerStatus.OutOfStock },
            new Retailer { Name = "Mane District", Url = "manedistrict.com", Status = RetailerStatus.OutOfStock },
            new Retailer { Name = "Crown & Glory Hair", Url = "crownandgloryhair.com", Status = RetailerStatus.InStock, DiscoveredAt = DateTime.UtcNow.AddDays(-5) },
            new Retailer { Name = "Silk Roots Boutique", Url = "silkrootsboutique.com", Status = RetailerStatus.Unknown, DiscoveredAt = DateTime.UtcNow.AddDays(-1) },
        };

        await context.Retailers.AddRangeAsync(retailers);
        await context.SaveChangesAsync();

        _logger.LogInformation("Seeded {Count} static retailers", retailers.Length);
    }

    private static string ExtractDomain(string url)
    {
        try
        {
            if (!url.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                url = "https://" + url;

            var uri = new Uri(url);
            return uri.Host.TrimStart('w', 'w', 'w', '.');
        }
        catch
        {
            return url;
        }
    }
}
