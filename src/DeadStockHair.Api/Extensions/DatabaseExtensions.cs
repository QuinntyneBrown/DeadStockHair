using DeadStockHair.Api.Data;
using DeadStockHair.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DeadStockHair.Api.Extensions;

public static class DatabaseExtensions
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        try
        {
            logger.LogInformation("Ensuring database is created and migrated...");
            await context.Database.MigrateAsync();
            logger.LogInformation("Database migration completed successfully");

            // Seed initial data if database is empty
            if (!await context.Retailers.AnyAsync())
            {
                logger.LogInformation("Seeding initial data...");
                await SeedDataAsync(context);
                logger.LogInformation("Data seeding completed successfully");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initializing the database");
            throw;
        }
    }

    private static async Task SeedDataAsync(ApplicationDbContext context)
    {
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
    }
}
