using System.CommandLine;
using DeadStockHair.Cli.Commands;
using DeadStockHair.Cli.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<IRetailerScraperService, RetailerScraperService>();
    })
    .Build();

var rootCommand = new RootCommand("CLI tool for finding online retailers selling dead stock hair");

rootCommand.AddCommand(ScanCommand.Create(host.Services));

return await rootCommand.InvokeAsync(args);
