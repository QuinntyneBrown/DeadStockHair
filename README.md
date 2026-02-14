# DeadStockHair

A .NET 9 solution for discovering and managing online retailers selling dead stock hair products.

## Components

### DeadStockHair.Api (Production-Ready Backend)

A production-ready ASP.NET Core Web API with:
- ✅ Entity Framework Core with SQLite/SQL Server support
- ✅ Dependency Injection with service interfaces
- ✅ Structured logging with OpenTelemetry
- ✅ Configuration management
- ✅ Database health checks
- ✅ RESTful API endpoints
- ✅ OpenAPI/Swagger documentation

[View API Documentation →](src/DeadStockHair.Api/README.md)

### DeadStockHair.Cli

A command-line tool that discovers online retailers selling dead stock hair using web scraping powered by Playwright.

## Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Playwright browsers (installed automatically on first run, or manually via the command below)

## Getting Started

### Running the API

```bash
# Navigate to the API project
cd src/DeadStockHair.Api

# Run the API (database will be created automatically)
dotnet run
```

The API will be available at `http://localhost:5128` with:
- API endpoints at `/api/*`
- Health checks at `/health` and `/alive`
- OpenAPI documentation at `/openapi/v1.json` (development only)

### Running the CLI

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
  DeadStockHair.Api/         # Production-ready Web API
    Data/                    # EF Core DbContext
    Endpoints/               # Minimal API endpoints
    Extensions/              # Database initialization
    Migrations/              # EF Core migrations
    Models/                  # Domain entities
    Services/                # Business logic with DI
    
  DeadStockHair.Cli/         # Command-line tool
    Commands/                # System.CommandLine command definitions
    Models/                  # Data models
    Services/                # Playwright scraping services
    
  DeadStockHair.AppHost/     # .NET Aspire orchestration
  
  DeadStockHair.ServiceDefaults/  # Shared service configuration
  
  DeadStockHair.Web/         # Angular frontend (separate)
```

## Tech Stack

### Backend API
- [.NET 9](https://dotnet.microsoft.com/) - Runtime and SDK
- [ASP.NET Core](https://docs.microsoft.com/aspnet/core) - Web framework
- [Entity Framework Core 9.0](https://docs.microsoft.com/ef/core) - ORM with SQLite/SQL Server
- [OpenTelemetry](https://opentelemetry.io/) - Observability and logging
- [Microsoft.Extensions.DependencyInjection](https://learn.microsoft.com/dotnet/core/extensions/dependency-injection) - Dependency injection

### CLI Tool
- [.NET 9](https://dotnet.microsoft.com/) - Runtime and SDK
- [System.CommandLine](https://github.com/dotnet/command-line-api) - CLI argument parsing
- [Microsoft.Extensions.Hosting](https://learn.microsoft.com/en-us/dotnet/core/extensions/generic-host) - DI, configuration, and logging
- [Microsoft.Playwright](https://playwright.dev/dotnet/) - Browser automation for web scraping

## License

This project is licensed under the MIT License. See [LICENSE](LICENSE) for details.
