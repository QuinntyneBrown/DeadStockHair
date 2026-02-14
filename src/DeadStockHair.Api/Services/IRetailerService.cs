using DeadStockHair.Api.Models;

namespace DeadStockHair.Api.Services;

public interface IRetailerService
{
    Task<IReadOnlyList<Retailer>> GetAllRetailersAsync();
    Task<IReadOnlyList<Retailer>> SearchRetailersAsync(string query);
    Task<Retailer?> GetRetailerAsync(Guid id);
    Task<Retailer> AddRetailerAsync(Retailer retailer);
    Task<bool> DeleteRetailerAsync(Guid id);
    Task<RetailerStats> GetStatsAsync();
    Task<IReadOnlyList<Retailer>> GetSavedRetailersAsync();
    Task<SavedRetailer?> SaveRetailerAsync(Guid retailerId);
    Task<bool> UnsaveRetailerAsync(Guid retailerId);
    Task<bool> IsRetailerSavedAsync(Guid retailerId);
    Task<ScanResult> CreateScanAsync();
    Task<ScanResult?> GetScanAsync(Guid id);
    Task<ScanResult?> GetLatestScanAsync();
    Task CompleteScanAsync(Guid id, int retailersFound);
    Task FailScanAsync(Guid id);
}
