using DeadStockHair.Api.Data;
using DeadStockHair.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DeadStockHair.Api.Services;

public class RetailerService : IRetailerService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<RetailerService> _logger;

    public RetailerService(ApplicationDbContext context, ILogger<RetailerService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IReadOnlyList<Retailer>> GetAllRetailersAsync()
    {
        _logger.LogInformation("Retrieving all retailers");
        return await _context.Retailers
            .OrderByDescending(r => r.DiscoveredAt)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Retailer>> SearchRetailersAsync(string query)
    {
        _logger.LogInformation("Searching retailers with query: {Query}", query);
        return await _context.Retailers
            .Where(r => EF.Functions.Like(r.Name, $"%{query}%") || EF.Functions.Like(r.Url, $"%{query}%"))
            .OrderByDescending(r => r.DiscoveredAt)
            .ToListAsync();
    }

    public async Task<Retailer?> GetRetailerAsync(Guid id)
    {
        _logger.LogInformation("Retrieving retailer with ID: {RetailerId}", id);
        return await _context.Retailers.FindAsync(id);
    }

    public async Task<Retailer> AddRetailerAsync(Retailer retailer)
    {
        _logger.LogInformation("Adding new retailer: {RetailerName}", retailer.Name);
        _context.Retailers.Add(retailer);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Successfully added retailer with ID: {RetailerId}", retailer.Id);
        return retailer;
    }

    public async Task<bool> DeleteRetailerAsync(Guid id)
    {
        _logger.LogInformation("Deleting retailer with ID: {RetailerId}", id);
        var retailer = await _context.Retailers.FindAsync(id);
        if (retailer == null)
        {
            _logger.LogWarning("Retailer with ID: {RetailerId} not found for deletion", id);
            return false;
        }

        _context.Retailers.Remove(retailer);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Successfully deleted retailer with ID: {RetailerId}", id);
        return true;
    }

    public async Task<RetailerStats> GetStatsAsync()
    {
        _logger.LogInformation("Calculating retailer statistics");
        
        var oneWeekAgo = DateTime.UtcNow.AddDays(-7);
        
        var totalRetailers = await _context.Retailers.CountAsync();
        var inStock = await _context.Retailers.CountAsync(r => r.Status == RetailerStatus.InStock);
        var newThisWeek = await _context.Retailers.CountAsync(r => r.DiscoveredAt >= oneWeekAgo);
        
        var stats = new RetailerStats(
            TotalRetailers: totalRetailers,
            InStock: inStock,
            NewThisWeek: newThisWeek
        );
        
        _logger.LogInformation("Retailer statistics calculated: {TotalRetailers} total, {InStock} in stock, {NewThisWeek} new this week", 
            stats.TotalRetailers, stats.InStock, stats.NewThisWeek);
        return stats;
    }

    public async Task<IReadOnlyList<Retailer>> GetSavedRetailersAsync()
    {
        _logger.LogInformation("Retrieving saved retailers");
        
        return await _context.Retailers
            .Where(r => _context.SavedRetailers.Any(s => s.RetailerId == r.Id))
            .OrderByDescending(r => r.DiscoveredAt)
            .ToListAsync();
    }

    public async Task<SavedRetailer?> SaveRetailerAsync(Guid retailerId)
    {
        _logger.LogInformation("Saving retailer with ID: {RetailerId}", retailerId);
        
        var retailer = await _context.Retailers.FindAsync(retailerId);
        if (retailer == null)
        {
            _logger.LogWarning("Retailer with ID: {RetailerId} not found", retailerId);
            return null;
        }

        var existing = await _context.SavedRetailers
            .FirstOrDefaultAsync(s => s.RetailerId == retailerId);
        
        if (existing != null)
        {
            _logger.LogInformation("Retailer with ID: {RetailerId} is already saved", retailerId);
            return existing;
        }

        var saved = new SavedRetailer { RetailerId = retailerId };
        _context.SavedRetailers.Add(saved);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Successfully saved retailer with ID: {RetailerId}", retailerId);
        return saved;
    }

    public async Task<bool> UnsaveRetailerAsync(Guid retailerId)
    {
        _logger.LogInformation("Unsaving retailer with ID: {RetailerId}", retailerId);
        
        var saved = await _context.SavedRetailers
            .FirstOrDefaultAsync(s => s.RetailerId == retailerId);
        
        if (saved == null)
        {
            _logger.LogWarning("No saved entry found for retailer ID: {RetailerId}", retailerId);
            return false;
        }

        _context.SavedRetailers.Remove(saved);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Successfully unsaved retailer with ID: {RetailerId}", retailerId);
        return true;
    }

    public async Task<bool> IsRetailerSavedAsync(Guid retailerId)
    {
        return await _context.SavedRetailers
            .AnyAsync(s => s.RetailerId == retailerId);
    }

    public async Task<ScanResult> CreateScanAsync()
    {
        _logger.LogInformation("Creating new scan");
        var scan = new ScanResult { Status = ScanStatus.Running };
        _context.ScanResults.Add(scan);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Created scan with ID: {ScanId}", scan.Id);
        return scan;
    }

    public async Task<ScanResult?> GetScanAsync(Guid id)
    {
        _logger.LogInformation("Retrieving scan with ID: {ScanId}", id);
        return await _context.ScanResults.FindAsync(id);
    }

    public async Task<ScanResult?> GetLatestScanAsync()
    {
        _logger.LogInformation("Retrieving latest scan");
        return await _context.ScanResults
            .OrderByDescending(s => s.StartedAt)
            .FirstOrDefaultAsync();
    }

    public async Task CompleteScanAsync(Guid id, int retailersFound)
    {
        _logger.LogInformation("Completing scan with ID: {ScanId}, retailers found: {RetailersFound}", id, retailersFound);
        
        var scan = await _context.ScanResults.FindAsync(id);
        if (scan != null)
        {
            scan.Status = ScanStatus.Completed;
            scan.CompletedAt = DateTime.UtcNow;
            scan.RetailersFound = retailersFound;
            await _context.SaveChangesAsync();
            _logger.LogInformation("Successfully completed scan with ID: {ScanId}", id);
        }
        else
        {
            _logger.LogWarning("Scan with ID: {ScanId} not found", id);
        }
    }

    public async Task FailScanAsync(Guid id)
    {
        _logger.LogInformation("Failing scan with ID: {ScanId}", id);
        
        var scan = await _context.ScanResults.FindAsync(id);
        if (scan != null)
        {
            scan.Status = ScanStatus.Failed;
            scan.CompletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            _logger.LogInformation("Successfully marked scan with ID: {ScanId} as failed", id);
        }
        else
        {
            _logger.LogWarning("Scan with ID: {ScanId} not found", id);
        }
    }
}
