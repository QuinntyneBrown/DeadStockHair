using DeadStockHair.Api.Data;
using DeadStockHair.Api.Services;
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

            // Use the SeedingService which tries CLI scraper first, then falls back to static data
            var seedingService = scope.ServiceProvider.GetRequiredService<SeedingService>();
            await seedingService.SeedAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initializing the database");
            throw;
        }
    }
}
