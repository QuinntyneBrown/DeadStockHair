using DeadStockHair.Cli.Models;

namespace DeadStockHair.Cli.Services;

public interface IRetailerScraperService
{
    Task<IEnumerable<Retailer>> ScanAsync(bool headless);
}
