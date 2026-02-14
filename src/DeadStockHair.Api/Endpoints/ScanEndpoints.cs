using DeadStockHair.Api.Models;
using DeadStockHair.Api.Services;

namespace DeadStockHair.Api.Endpoints;

public static class ScanEndpoints
{
    public static RouteGroupBuilder MapScanEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/scan").WithTags("Scan");

        group.MapPost("/", async (IRetailerService service, ILogger<Program> logger) =>
        {
            var scan = await service.CreateScanAsync();

            // Simulate async scan completion in the background
            _ = Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(3000);

                    var newRetailers = new[]
                    {
                        new Retailer { Name = "Belle Hair Studio", Url = "bellehair.com", Status = RetailerStatus.InStock },
                        new Retailer { Name = "Strand Supply Co", Url = "strandsupply.com", Status = RetailerStatus.Unknown },
                    };

                    foreach (var retailer in newRetailers)
                    {
                        await service.AddRetailerAsync(retailer);
                    }

                    await service.CompleteScanAsync(scan.Id, newRetailers.Length);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error completing scan {ScanId}", scan.Id);
                    await service.FailScanAsync(scan.Id);
                }
            });

            return Results.Accepted($"/api/scan/{scan.Id}", scan);
        })
        .WithName("StartScan")
        .WithSummary("Trigger a new retailer scan");

        group.MapGet("/{id:guid}", async (Guid id, IRetailerService service) =>
        {
            var scan = await service.GetScanAsync(id);
            return scan is not null ? Results.Ok(scan) : Results.NotFound();
        })
        .WithName("GetScanStatus")
        .WithSummary("Get the status of a scan");

        group.MapGet("/latest", async (IRetailerService service) =>
        {
            var scan = await service.GetLatestScanAsync();
            return scan is not null ? Results.Ok(scan) : Results.NotFound();
        })
        .WithName("GetLatestScan")
        .WithSummary("Get the most recent scan result");

        return group;
    }
}
