using System.CommandLine;
using DeadStockHair.Cli.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DeadStockHair.Cli.Commands;

public static class ScanCommand
{
    public static Command Create(IServiceProvider services)
    {
        var headlessOption = new Option<bool>(
            "--headless",
            getDefaultValue: () => true,
            description: "Run the browser in headless mode");

        var command = new Command("scan", "Search for online retailers selling dead stock hair")
        {
            headlessOption
        };

        command.SetHandler(async (bool headless) =>
        {
            var scraperService = services.GetRequiredService<IRetailerScraperService>();

            var retailers = await scraperService.ScanAsync(headless);

            var retailerList = retailers.ToList();

            if (retailerList.Count == 0)
            {
                Console.WriteLine("No retailers found.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine($"  {"Name",-50} {"URL"}");
            Console.WriteLine($"  {new string('-', 50)} {new string('-', 60)}");

            foreach (var retailer in retailerList)
            {
                Console.WriteLine($"  {retailer.Name,-50} {retailer.Url}");
            }

            Console.WriteLine();
            Console.WriteLine($"  Found {retailerList.Count} retailer(s).");
            Console.WriteLine();
        }, headlessOption);

        return command;
    }
}
