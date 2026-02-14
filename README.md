# DeadStockHair CLI

A command-line tool that discovers online retailers selling dead stock hair using web scraping powered by Playwright.

## Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Playwright browsers (installed automatically on first run, or manually via the command below)

## Getting Started

```bash
# Clone the repository
git clone https://github.com/QuinntyneBrown/DeadStockHair.git
cd DeadStockHair

# Build the project
dotnet build src/DeadStockHair.Cli

# Install Playwright browsers
pwsh src/DeadStockHair.Cli/bin/Debug/net9.0/playwright.ps1 install
```

## Usage

```bash
# Run the scan command (headless by default)
dotnet run --project src/DeadStockHair.Cli -- scan

# Run with a visible browser window
dotnet run --project src/DeadStockHair.Cli -- scan --headless false
```

### Install as a global tool

```bash
dotnet pack src/DeadStockHair.Cli
dotnet tool install --global --add-source src/DeadStockHair.Cli/nupkg DeadStockHair.Cli

# Then use it directly
deadstock scan
```

## Project Structure

```
src/
  DeadStockHair.Cli/
    Commands/        # System.CommandLine command definitions
    Models/          # Data models (Retailer record)
    Services/        # Business logic and Playwright scraping (IRetailerScraperService, RetailerScraperService)
    Program.cs       # Host builder and DI setup
```

## Tech Stack

- [.NET 9](https://dotnet.microsoft.com/) - Runtime and SDK
- [System.CommandLine](https://github.com/dotnet/command-line-api) - CLI argument parsing
- [Microsoft.Extensions.Hosting](https://learn.microsoft.com/en-us/dotnet/core/extensions/generic-host) - DI, configuration, and logging
- [Microsoft.Playwright](https://playwright.dev/dotnet/) - Browser automation for web scraping

## License

This project is licensed under the MIT License. See [LICENSE](LICENSE) for details.
