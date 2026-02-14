using DeadStockHair.Api.Services;

namespace DeadStockHair.Api.Endpoints;

public static class SavedEndpoints
{
    public static RouteGroupBuilder MapSavedEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/saved").WithTags("Saved");

        group.MapGet("/", async (IRetailerService service) =>
        {
            return Results.Ok(await service.GetSavedRetailersAsync());
        })
        .WithName("GetSavedRetailers")
        .WithSummary("Get all saved/bookmarked retailers");

        group.MapPost("/{retailerId:guid}", async (Guid retailerId, IRetailerService service) =>
        {
            var saved = await service.SaveRetailerAsync(retailerId);
            return saved is not null
                ? Results.Ok(saved)
                : Results.NotFound(new { message = "Retailer not found" });
        })
        .WithName("SaveRetailer")
        .WithSummary("Save/bookmark a retailer");

        group.MapDelete("/{retailerId:guid}", async (Guid retailerId, IRetailerService service) =>
        {
            return await service.UnsaveRetailerAsync(retailerId) ? Results.NoContent() : Results.NotFound();
        })
        .WithName("UnsaveRetailer")
        .WithSummary("Remove a saved retailer");

        group.MapGet("/{retailerId:guid}/status", async (Guid retailerId, IRetailerService service) =>
        {
            return Results.Ok(new { isSaved = await service.IsRetailerSavedAsync(retailerId) });
        })
        .WithName("GetSavedStatus")
        .WithSummary("Check if a retailer is saved");

        return group;
    }
}
