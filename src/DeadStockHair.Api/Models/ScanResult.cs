namespace DeadStockHair.Api.Models;

public class ScanResult
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public ScanStatus Status { get; set; } = ScanStatus.Pending;
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    public int RetailersFound { get; set; }
}

public enum ScanStatus
{
    Pending,
    Running,
    Completed,
    Failed
}
