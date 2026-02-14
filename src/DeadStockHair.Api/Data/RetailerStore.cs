using System.Collections.Concurrent;
using DeadStockHair.Api.Models;

namespace DeadStockHair.Api.Data;

public class RetailerStore
{
    private readonly ConcurrentDictionary<Guid, Retailer> _retailers = new();
    private readonly ConcurrentDictionary<Guid, SavedRetailer> _savedRetailers = new();
    private readonly ConcurrentDictionary<Guid, ScanResult> _scans = new();

    public RetailerStore()
    {
        SeedData();
    }

    private void SeedData()
    {
        var retailers = new[]
        {
            new Retailer { Name = "DROP DEAD Extensions", Url = "dropdeadextensions.com", Status = RetailerStatus.InStock },
            new Retailer { Name = "Hair Overstock", Url = "hairoverstock.com", Status = RetailerStatus.InStock },
            new Retailer { Name = "Viola Hair Extensions", Url = "violahairextensions.co", Status = RetailerStatus.InStock },
            new Retailer { Name = "ParaHair Canada", Url = "parahair.ca", Status = RetailerStatus.InStock },
            new Retailer { Name = "Golden Lush Extensions", Url = "goldenlushextensions.com", Status = RetailerStatus.InStock },
            new Retailer { Name = "Chiquel Hair", Url = "chiquel.ca", Status = RetailerStatus.InStock, DiscoveredAt = DateTime.UtcNow.AddDays(-2) },
            new Retailer { Name = "Luxe Strand Co", Url = "luxestrand.com", Status = RetailerStatus.OutOfStock },
            new Retailer { Name = "Mane District", Url = "manedistrict.com", Status = RetailerStatus.OutOfStock },
            new Retailer { Name = "Crown & Glory Hair", Url = "crownandgloryhair.com", Status = RetailerStatus.InStock, DiscoveredAt = DateTime.UtcNow.AddDays(-5) },
            new Retailer { Name = "Silk Roots Boutique", Url = "silkrootsboutique.com", Status = RetailerStatus.Unknown, DiscoveredAt = DateTime.UtcNow.AddDays(-1) },
        };

        foreach (var retailer in retailers)
        {
            _retailers[retailer.Id] = retailer;
        }
    }

    // Retailers
    public IReadOnlyList<Retailer> GetAllRetailers() =>
        _retailers.Values.OrderByDescending(r => r.DiscoveredAt).ToList();

    public IReadOnlyList<Retailer> SearchRetailers(string query) =>
        _retailers.Values
            .Where(r => r.Name.Contains(query, StringComparison.OrdinalIgnoreCase)
                     || r.Url.Contains(query, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(r => r.DiscoveredAt)
            .ToList();

    public Retailer? GetRetailer(Guid id) =>
        _retailers.GetValueOrDefault(id);

    public Retailer AddRetailer(Retailer retailer)
    {
        _retailers[retailer.Id] = retailer;
        return retailer;
    }

    public bool DeleteRetailer(Guid id) =>
        _retailers.TryRemove(id, out _);

    public RetailerStats GetStats()
    {
        var all = _retailers.Values.ToList();
        var oneWeekAgo = DateTime.UtcNow.AddDays(-7);
        return new RetailerStats(
            TotalRetailers: all.Count,
            InStock: all.Count(r => r.Status == RetailerStatus.InStock),
            NewThisWeek: all.Count(r => r.DiscoveredAt >= oneWeekAgo)
        );
    }

    // Saved Retailers
    public IReadOnlyList<Retailer> GetSavedRetailers()
    {
        var savedIds = _savedRetailers.Values.Select(s => s.RetailerId).ToHashSet();
        return _retailers.Values
            .Where(r => savedIds.Contains(r.Id))
            .OrderByDescending(r => r.DiscoveredAt)
            .ToList();
    }

    public SavedRetailer? SaveRetailer(Guid retailerId)
    {
        if (!_retailers.ContainsKey(retailerId))
            return null;

        if (_savedRetailers.Values.Any(s => s.RetailerId == retailerId))
            return _savedRetailers.Values.First(s => s.RetailerId == retailerId);

        var saved = new SavedRetailer { RetailerId = retailerId };
        _savedRetailers[saved.Id] = saved;
        return saved;
    }

    public bool UnsaveRetailer(Guid retailerId)
    {
        var saved = _savedRetailers.Values.FirstOrDefault(s => s.RetailerId == retailerId);
        if (saved == null) return false;
        return _savedRetailers.TryRemove(saved.Id, out _);
    }

    public bool IsRetailerSaved(Guid retailerId) =>
        _savedRetailers.Values.Any(s => s.RetailerId == retailerId);

    // Scans
    public ScanResult CreateScan()
    {
        var scan = new ScanResult { Status = ScanStatus.Running };
        _scans[scan.Id] = scan;
        return scan;
    }

    public ScanResult? GetScan(Guid id) =>
        _scans.GetValueOrDefault(id);

    public ScanResult? GetLatestScan() =>
        _scans.Values.OrderByDescending(s => s.StartedAt).FirstOrDefault();

    public void CompleteScan(Guid id, int retailersFound)
    {
        if (_scans.TryGetValue(id, out var scan))
        {
            scan.Status = ScanStatus.Completed;
            scan.CompletedAt = DateTime.UtcNow;
            scan.RetailersFound = retailersFound;
        }
    }

    public void FailScan(Guid id)
    {
        if (_scans.TryGetValue(id, out var scan))
        {
            scan.Status = ScanStatus.Failed;
            scan.CompletedAt = DateTime.UtcNow;
        }
    }
}
