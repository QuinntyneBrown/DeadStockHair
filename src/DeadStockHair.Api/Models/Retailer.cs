namespace DeadStockHair.Api.Models;

public class Retailer
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public required string Url { get; set; }
    public RetailerStatus Status { get; set; } = RetailerStatus.InStock;
    public DateTime DiscoveredAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastCheckedAt { get; set; }
}

public enum RetailerStatus
{
    InStock,
    OutOfStock,
    Unknown
}
