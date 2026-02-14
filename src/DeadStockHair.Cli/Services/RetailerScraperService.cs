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
            Headless = headless,
            Args = ["--disable-blink-features=AutomationControlled"]
        });

        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/131.0.0.0 Safari/537.36"
        });

        var page = await context.NewPageAsync();
        await page.AddInitScriptAsync("Object.defineProperty(navigator, 'webdriver', { get: () => undefined });");

        var retailers = new List<Retailer>();

        // Use DuckDuckGo HTML-only version (no JS required, most reliable for scraping)
        retailers = await ScrapeDuckDuckGoHtmlAsync(page);

        if (retailers.Count == 0)
        {
            _logger.LogWarning("DuckDuckGo HTML returned no results. Trying Google...");
            retailers = await ScrapeGoogleAsync(page);
        }

        _logger.LogInformation("Found {Count} retailer(s) total", retailers.Count);

        return retailers
            .DistinctBy(r => r.Url)
            .ToList();
    }

    private async Task<List<Retailer>> ScrapeDuckDuckGoHtmlAsync(IPage page)
    {
        _logger.LogInformation("Searching DuckDuckGo (HTML) for dead stock hair retailers...");

        try
        {
            await page.GotoAsync("https://html.duckduckgo.com/html/?q=buy+dead+stock+hair+extensions+online", new PageGotoOptions
            {
                WaitUntil = WaitUntilState.DOMContentLoaded,
                Timeout = 30000
            });

            var html = await page.ContentAsync();
            await File.WriteAllTextAsync("debug-ddg.html", html);
            _logger.LogInformation("DDG page URL: {Url}, HTML length: {Length}", page.Url, html.Length);

            var results = await page.QuerySelectorAllAsync(".result");
            _logger.LogInformation("DuckDuckGo HTML: found {Count} result elements", results.Count);

            var retailers = new List<Retailer>();

            foreach (var result in results)
            {
                var anchor = await result.QuerySelectorAsync(".result__a");
                if (anchor == null) continue;

                var name = (await anchor.InnerTextAsync()).Trim();
                var url = await anchor.GetAttributeAsync("href");

                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(url)) continue;
                if (url.Contains("duckduckgo.com")) continue;

                // DuckDuckGo HTML redirects through their tracking URL, extract the actual URL
                if (url.Contains("uddg="))
                {
                    var uri = new Uri("https://duckduckgo.com" + url);
                    var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
                    url = query["uddg"] ?? url;
                }

                retailers.Add(new Retailer(name, url));
            }

            return retailers;
        }
        catch (Exception ex)
        {
            _logger.LogWarning("DuckDuckGo HTML scraping failed: {Message}", ex.Message);
            return [];
        }
    }

    private async Task<List<Retailer>> ScrapeGoogleAsync(IPage page)
    {
        _logger.LogInformation("Searching Google for dead stock hair retailers...");

        try
        {
            await page.GotoAsync("https://www.google.com/search?q=buy+dead+stock+hair+extensions+online", new PageGotoOptions
            {
                WaitUntil = WaitUntilState.NetworkIdle,
                Timeout = 30000
            });

            await page.WaitForSelectorAsync("#search", new PageWaitForSelectorOptions
            {
                Timeout = 10000
            });

            var results = await page.QuerySelectorAllAsync("#search .g");

            var retailers = new List<Retailer>();

            foreach (var result in results)
            {
                var anchor = await result.QuerySelectorAsync("a");
                if (anchor == null) continue;

                var url = await anchor.GetAttributeAsync("href");
                if (string.IsNullOrWhiteSpace(url) || !url.StartsWith("http")) continue;
                if (url.Contains("google.com")) continue;

                var h3 = await result.QuerySelectorAsync("h3");
                var name = h3 != null ? (await h3.InnerTextAsync()).Trim() : "";

                if (string.IsNullOrWhiteSpace(name)) continue;

                retailers.Add(new Retailer(name, url));
            }

            return retailers;
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Google scraping failed: {Message}", ex.Message);
            return [];
        }
    }
}
