namespace DeadStockHair.Api.Models;

public class SavedRetailer
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid RetailerId { get; set; }
    public DateTime SavedAt { get; set; } = DateTime.UtcNow;
}
