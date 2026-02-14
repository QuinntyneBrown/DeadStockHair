using DeadStockHair.Api.Data;

namespace DeadStockHair.Api.Endpoints;

public static class SavedEndpoints
{
    public static RouteGroupBuilder MapSavedEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/saved").WithTags("Saved");

        group.MapGet("/", (RetailerStore store) =>
        {
            return Results.Ok(store.GetSavedRetailers());
        })
        .WithName("GetSavedRetailers")
        .WithSummary("Get all saved/bookmarked retailers");

        group.MapPost("/{retailerId:guid}", (Guid retailerId, RetailerStore store) =>
        {
            var saved = store.SaveRetailer(retailerId);
            return saved is not null
                ? Results.Ok(saved)
                : Results.NotFound(new { message = "Retailer not found" });
        })
        .WithName("SaveRetailer")
        .WithSummary("Save/bookmark a retailer");

        group.MapDelete("/{retailerId:guid}", (Guid retailerId, RetailerStore store) =>
        {
            return store.UnsaveRetailer(retailerId) ? Results.NoContent() : Results.NotFound();
        })
        .WithName("UnsaveRetailer")
        .WithSummary("Remove a saved retailer");

        group.MapGet("/{retailerId:guid}/status", (Guid retailerId, RetailerStore store) =>
        {
            return Results.Ok(new { isSaved = store.IsRetailerSaved(retailerId) });
        })
        .WithName("GetSavedStatus")
        .WithSummary("Check if a retailer is saved");

        return group;
    }
}
