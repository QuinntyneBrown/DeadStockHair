using DeadStockHair.Api.Data;
using DeadStockHair.Api.Models;

namespace DeadStockHair.Api.Endpoints;

public static class RetailerEndpoints
{
    public static RouteGroupBuilder MapRetailerEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/retailers").WithTags("Retailers");

        group.MapGet("/", (RetailerStore store, string? search) =>
        {
            var retailers = string.IsNullOrWhiteSpace(search)
                ? store.GetAllRetailers()
                : store.SearchRetailers(search);

            return Results.Ok(retailers);
        })
        .WithName("GetRetailers")
        .WithSummary("Get all retailers, optionally filtered by search query");

        group.MapGet("/stats", (RetailerStore store) =>
        {
            return Results.Ok(store.GetStats());
        })
        .WithName("GetRetailerStats")
        .WithSummary("Get retailer statistics");

        group.MapGet("/{id:guid}", (Guid id, RetailerStore store) =>
        {
            var retailer = store.GetRetailer(id);
            return retailer is not null ? Results.Ok(retailer) : Results.NotFound();
        })
        .WithName("GetRetailerById")
        .WithSummary("Get a retailer by ID");

        group.MapPost("/", (CreateRetailerRequest request, RetailerStore store) =>
        {
            var retailer = new Retailer
            {
                Name = request.Name,
                Url = request.Url,
                Status = request.Status ?? RetailerStatus.Unknown
            };

            store.AddRetailer(retailer);
            return Results.Created($"/api/retailers/{retailer.Id}", retailer);
        })
        .WithName("CreateRetailer")
        .WithSummary("Add a new retailer");

        group.MapDelete("/{id:guid}", (Guid id, RetailerStore store) =>
        {
            return store.DeleteRetailer(id) ? Results.NoContent() : Results.NotFound();
        })
        .WithName("DeleteRetailer")
        .WithSummary("Delete a retailer");

        return group;
    }
}

public record CreateRetailerRequest(string Name, string Url, RetailerStatus? Status = null);
