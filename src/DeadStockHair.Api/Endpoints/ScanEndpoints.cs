using DeadStockHair.Api.Data;
using DeadStockHair.Api.Models;

namespace DeadStockHair.Api.Endpoints;

public static class ScanEndpoints
{
    public static RouteGroupBuilder MapScanEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/scan").WithTags("Scan");

        group.MapPost("/", (RetailerStore store) =>
        {
            var scan = store.CreateScan();

            // Simulate async scan completion in the background
            _ = Task.Run(async () =>
            {
                await Task.Delay(3000);

                var newRetailers = new[]
                {
                    new Retailer { Name = "Belle Hair Studio", Url = "bellehair.com", Status = RetailerStatus.InStock },
                    new Retailer { Name = "Strand Supply Co", Url = "strandsupply.com", Status = RetailerStatus.Unknown },
                };

                foreach (var retailer in newRetailers)
                {
                    store.AddRetailer(retailer);
                }

                store.CompleteScan(scan.Id, newRetailers.Length);
            });

            return Results.Accepted($"/api/scan/{scan.Id}", scan);
        })
        .WithName("StartScan")
        .WithSummary("Trigger a new retailer scan");

        group.MapGet("/{id:guid}", (Guid id, RetailerStore store) =>
        {
            var scan = store.GetScan(id);
            return scan is not null ? Results.Ok(scan) : Results.NotFound();
        })
        .WithName("GetScanStatus")
        .WithSummary("Get the status of a scan");

        group.MapGet("/latest", (RetailerStore store) =>
        {
            var scan = store.GetLatestScan();
            return scan is not null ? Results.Ok(scan) : Results.NotFound();
        })
        .WithName("GetLatestScan")
        .WithSummary("Get the most recent scan result");

        return group;
    }
}
