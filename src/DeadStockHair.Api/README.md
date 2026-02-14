# DeadStockHair API

A production-ready ASP.NET Core Web API for managing dead stock hair retailers.

## Features

- ✅ **Dependency Injection (DI)**: Properly configured with service interfaces and implementations
- ✅ **Structured Logging**: Comprehensive logging using Microsoft.Extensions.Logging with OpenTelemetry
- ✅ **Configuration Management**: Environment-specific settings with appsettings.json
- ✅ **Real Database Support**: Entity Framework Core with SQLite and SQL Server providers
- ✅ **Health Checks**: Database and application health monitoring
- ✅ **API Documentation**: OpenAPI/Swagger support
- ✅ **CORS**: Configured for cross-origin requests

## Database Configuration

The API supports both SQLite (default) and SQL Server databases. You can configure which provider to use in `appsettings.json`:

### SQLite (Development - Default)

```json
{
  "DatabaseProvider": "Sqlite",
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=deadstockhair.db"
  }
}
```

### SQL Server (Production)

```json
{
  "DatabaseProvider": "SqlServer",
  "ConnectionStrings": {
    "SqlServerConnection": "Server=(localdb)\\mssqllocaldb;Database=DeadStockHairDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

Or use environment variables:

```bash
export DatabaseProvider="SqlServer"
export ConnectionStrings__SqlServerConnection="Server=myserver;Database=DeadStockHairDb;User Id=sa;Password=YourPassword;"
```

## Running the API

### Development

```bash
cd src/DeadStockHair.Api
dotnet run
```

The API will:
1. Automatically create the database
2. Run migrations
3. Seed initial data
4. Start listening on `http://localhost:5128`

### Production

Set the environment to Production:

```bash
export ASPNETCORE_ENVIRONMENT=Production
dotnet run
```

Or using Docker:

```bash
docker build -t deadstockhair-api .
docker run -p 5000:8080 -e ASPNETCORE_ENVIRONMENT=Production deadstockhair-api
```

## Database Migrations

### Create a new migration

```bash
cd src/DeadStockHair.Api
dotnet ef migrations add YourMigrationName
```

### Apply migrations

Migrations are applied automatically on startup. To apply manually:

```bash
dotnet ef database update
```

## API Endpoints

### Retailers

- `GET /api/retailers` - Get all retailers (with optional search query)
- `GET /api/retailers/{id}` - Get a specific retailer
- `GET /api/retailers/stats` - Get retailer statistics
- `POST /api/retailers` - Create a new retailer
- `DELETE /api/retailers/{id}` - Delete a retailer

### Saved Retailers

- `GET /api/saved` - Get all saved retailers
- `POST /api/saved/{retailerId}` - Save a retailer
- `DELETE /api/saved/{retailerId}` - Unsave a retailer
- `GET /api/saved/{retailerId}/status` - Check if retailer is saved

### Scans

- `POST /api/scan` - Start a new scan
- `GET /api/scan/{id}` - Get scan status
- `GET /api/scan/latest` - Get latest scan result

### Health

- `GET /health` - Application health check
- `GET /alive` - Liveness check

## Architecture

```
DeadStockHair.Api/
├── Data/
│   ├── ApplicationDbContext.cs      # EF Core DbContext
│   └── RetailerStore.cs             # Legacy in-memory store (deprecated)
├── Endpoints/
│   ├── RetailerEndpoints.cs         # Retailer API endpoints
│   ├── SavedEndpoints.cs            # Saved retailers endpoints
│   └── ScanEndpoints.cs             # Scan endpoints
├── Extensions/
│   └── DatabaseExtensions.cs        # Database initialization
├── Migrations/                       # EF Core migrations
├── Models/
│   ├── Retailer.cs                  # Retailer entity
│   ├── SavedRetailer.cs             # SavedRetailer entity
│   └── ScanResult.cs                # ScanResult entity
├── Services/
│   ├── IRetailerService.cs          # Service interface
│   └── RetailerService.cs           # Service implementation with logging
└── Program.cs                        # Application startup
```

## Logging

The API uses structured logging with different log levels per environment:

**Development:**
- Default: Information
- Microsoft.AspNetCore: Warning
- Microsoft.EntityFrameworkCore: Warning

**Production:**
- Default: Warning
- DeadStockHair: Information
- Microsoft.EntityFrameworkCore: Error

Logs include:
- Request/response information
- Database operations
- Error details with stack traces
- Performance metrics via OpenTelemetry

## Environment Variables

Key environment variables:

- `ASPNETCORE_ENVIRONMENT` - Environment name (Development, Production)
- `DatabaseProvider` - Database provider (Sqlite, SqlServer)
- `ConnectionStrings__DefaultConnection` - SQLite connection string
- `ConnectionStrings__SqlServerConnection` - SQL Server connection string

## Tech Stack

- .NET 9.0
- ASP.NET Core
- Entity Framework Core 9.0
- SQLite (development)
- SQL Server (production)
- OpenTelemetry for observability
- Microsoft.Extensions.Logging
