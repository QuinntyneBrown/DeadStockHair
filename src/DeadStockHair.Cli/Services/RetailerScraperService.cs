using DeadStockHair.Cli.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;

namespace DeadStockHair.Cli.Services;

public class RetailerScraperService : IRetailerScraperService
{
    private readonly ILogger<RetailerScraperService> _logger;

    public RetailerScraperService(ILogger<RetailerScraperService> logger)
    {
        _logger = logger;
    }

    public async Task<IEnumerable<Retailer>> ScanAsync(bool headless)
    {
        _logger.LogInformation("Launching browser (headless: {Headless})", headless);

        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = headless
        });

        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/131.0.0.0 Safari/537.36"
        });

        var page = await context.NewPageAsync();

        _logger.LogInformation("Searching for dead stock hair retailers...");

        await page.GotoAsync("https://www.bing.com/search?q=buy+dead+stock+hair+online", new PageGotoOptions
        {
            WaitUntil = WaitUntilState.NetworkIdle
        });

        // Handle Bing cookie consent if it appears
        var acceptButton = await page.QuerySelectorAsync("#bnp_btn_accept");
        if (acceptButton != null)
        {
            _logger.LogInformation("Accepting cookie consent...");
            await acceptButton.ClickAsync();
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        // Wait for search results to be visible
        await page.WaitForSelectorAsync("#b_results li.b_algo", new PageWaitForSelectorOptions
        {
            State = WaitForSelectorState.Attached,
            Timeout = 15000
        });

        var results = await page.QuerySelectorAllAsync("#b_results > li.b_algo");

        var retailers = new List<Retailer>();

        foreach (var result in results)
        {
            var anchor = await result.QuerySelectorAsync("h2 a");
            if (anchor == null) continue;

            var name = (await anchor.InnerTextAsync()).Trim();
            var url = await anchor.GetAttributeAsync("href");

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(url)) continue;

            retailers.Add(new Retailer(name, url));
        }

        _logger.LogInformation("Found {Count} results", retailers.Count);

        return retailers
            .DistinctBy(r => r.Url)
            .ToList();
    }
}
