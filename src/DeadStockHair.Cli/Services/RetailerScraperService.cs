using System.Text;
using System.Web;
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

        // Use Firefox - better at avoiding bot detection than Chromium
        await using var browser = await playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = headless
        });

        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize { Width = 1920, Height = 1080 },
            Locale = "en-US"
        });

        var page = await context.NewPageAsync();

        var allRetailers = new List<Retailer>();
        string[] queries = ["buy dead stock hair extensions online", "deadstock hair bundles for sale"];

        foreach (var query in queries)
        {
            var results = await ScrapeBingAsync(page, query);

            if (results.Count == 0)
            {
                results = await ScrapeGoogleAsync(page, query);
            }

            allRetailers.AddRange(results);
        }

        var deduplicated = allRetailers
            .DistinctBy(r => r.Url)
            .ToList();

        _logger.LogInformation("Found {Count} unique retailer(s) total", deduplicated.Count);

        return deduplicated;
    }

    private async Task<List<Retailer>> ScrapeBingAsync(IPage page, string query)
    {
        _logger.LogInformation("Searching Bing for: {Query}", query);

        try
        {
            var encodedQuery = Uri.EscapeDataString(query);
            await page.GotoAsync($"https://www.bing.com/search?q={encodedQuery}", new PageGotoOptions
            {
                WaitUntil = WaitUntilState.DOMContentLoaded,
                Timeout = 30000
            });

            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            // Handle Bing cookie consent
            try
            {
                var acceptButton = await page.QuerySelectorAsync("#bnp_btn_accept");
                if (acceptButton != null)
                {
                    await acceptButton.ClickAsync();
                    await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
                }
            }
            catch { /* No consent dialog */ }

            // Wait for search results
            try
            {
                await page.WaitForSelectorAsync("li.b_algo", new PageWaitForSelectorOptions
                {
                    State = WaitForSelectorState.Attached,
                    Timeout = 15000
                });
            }
            catch (TimeoutException)
            {
                _logger.LogWarning("Bing: timed out waiting for results");
                return [];
            }

            var results = await page.QuerySelectorAllAsync("li.b_algo");
            var retailers = new List<Retailer>();

            foreach (var result in results)
            {
                var anchor = await result.QuerySelectorAsync("h2 a");
                if (anchor == null) continue;

                var name = (await anchor.InnerTextAsync()).Trim();
                var url = await anchor.GetAttributeAsync("href");

                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(url)) continue;

                // Decode Bing tracking redirect URLs to get the actual destination
                url = DecodeBingUrl(url);

                retailers.Add(new Retailer(name, url));
            }

            _logger.LogInformation("Bing: found {Count} results", retailers.Count);
            return retailers;
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Bing scraping failed: {Message}", ex.Message);
            return [];
        }
    }

    private async Task<List<Retailer>> ScrapeGoogleAsync(IPage page, string query)
    {
        _logger.LogInformation("Searching Google for: {Query}", query);

        try
        {
            var encodedQuery = Uri.EscapeDataString(query);
            await page.GotoAsync($"https://www.google.com/search?q={encodedQuery}", new PageGotoOptions
            {
                WaitUntil = WaitUntilState.DOMContentLoaded,
                Timeout = 30000
            });

            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            // Handle Google consent dialog
            try
            {
                var consentButton = await page.QuerySelectorAsync("button[id='L2AGLb']");
                if (consentButton != null)
                {
                    await consentButton.ClickAsync();
                    await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
                }
            }
            catch { /* Consent dialog not present */ }

            try
            {
                await page.WaitForSelectorAsync("#search", new PageWaitForSelectorOptions
                {
                    State = WaitForSelectorState.Attached,
                    Timeout = 10000
                });
            }
            catch (TimeoutException)
            {
                return [];
            }

            var retailers = new List<Retailer>();
            var results = await page.QuerySelectorAllAsync("#search .g");

            foreach (var result in results)
            {
                var retailer = await ExtractGoogleResult(result);
                if (retailer != null) retailers.Add(retailer);
            }

            _logger.LogInformation("Google: found {Count} results", retailers.Count);
            return retailers;
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Google scraping failed: {Message}", ex.Message);
            return [];
        }
    }

    private static string DecodeBingUrl(string url)
    {
        if (!url.Contains("bing.com/ck/a")) return url;

        try
        {
            var uri = new Uri(url);
            var queryParams = HttpUtility.ParseQueryString(uri.Query);
            var encodedUrl = queryParams["u"];

            if (string.IsNullOrWhiteSpace(encodedUrl)) return url;

            // Bing uses a1 prefix + base64 encoding
            if (encodedUrl.StartsWith("a1"))
            {
                encodedUrl = encodedUrl[2..];
            }

            // Add padding if needed
            var padded = encodedUrl.PadRight(encodedUrl.Length + (4 - encodedUrl.Length % 4) % 4, '=');
            var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(padded));
            return decoded;
        }
        catch
        {
            return url;
        }
    }

    private static async Task<Retailer?> ExtractGoogleResult(IElementHandle result)
    {
        var anchor = await result.QuerySelectorAsync("a");
        if (anchor == null) return null;

        var url = await anchor.GetAttributeAsync("href");
        if (string.IsNullOrWhiteSpace(url) || !url.StartsWith("http")) return null;
        if (url.Contains("google.com")) return null;

        var h3 = await result.QuerySelectorAsync("h3");
        var name = h3 != null ? (await h3.InnerTextAsync()).Trim() : "";

        if (string.IsNullOrWhiteSpace(name)) return null;

        return new Retailer(name, url);
    }
}
