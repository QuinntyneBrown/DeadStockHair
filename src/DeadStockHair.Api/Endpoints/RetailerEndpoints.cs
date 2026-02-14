using DeadStockHair.Api.Models;
using DeadStockHair.Api.Services;

namespace DeadStockHair.Api.Endpoints;

public static class RetailerEndpoints
{
    public static RouteGroupBuilder MapRetailerEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/retailers").WithTags("Retailers");

        group.MapGet("/", async (IRetailerService service, string? search) =>
        {
            var retailers = string.IsNullOrWhiteSpace(search)
                ? await service.GetAllRetailersAsync()
                : await service.SearchRetailersAsync(search);

            return Results.Ok(retailers);
        })
        .WithName("GetRetailers")
        .WithSummary("Get all retailers, optionally filtered by search query");

        group.MapGet("/stats", async (IRetailerService service) =>
        {
            return Results.Ok(await service.GetStatsAsync());
        })
        .WithName("GetRetailerStats")
        .WithSummary("Get retailer statistics");

        group.MapGet("/{id:guid}", async (Guid id, IRetailerService service) =>
        {
            var retailer = await service.GetRetailerAsync(id);
            return retailer is not null ? Results.Ok(retailer) : Results.NotFound();
        })
        .WithName("GetRetailerById")
        .WithSummary("Get a retailer by ID");

        group.MapPost("/", async (CreateRetailerRequest request, IRetailerService service) =>
        {
            var retailer = new Retailer
            {
                Name = request.Name,
                Url = request.Url,
                Status = request.Status ?? RetailerStatus.Unknown
            };

            await service.AddRetailerAsync(retailer);
            return Results.Created($"/api/retailers/{retailer.Id}", retailer);
        })
        .WithName("CreateRetailer")
        .WithSummary("Add a new retailer");

        group.MapDelete("/{id:guid}", async (Guid id, IRetailerService service) =>
        {
            return await service.DeleteRetailerAsync(id) ? Results.NoContent() : Results.NotFound();
        })
        .WithName("DeleteRetailer")
        .WithSummary("Delete a retailer");

        return group;
    }
}

public record CreateRetailerRequest(string Name, string Url, RetailerStatus? Status = null);
