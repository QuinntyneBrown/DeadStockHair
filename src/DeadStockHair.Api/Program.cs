using DeadStockHair.Api.Data;
using DeadStockHair.Api.Endpoints;
using DeadStockHair.Api.Extensions;
using DeadStockHair.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add database context with provider selection
var databaseProvider = builder.Configuration.GetValue<string>("DatabaseProvider") ?? "Sqlite";
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (databaseProvider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
{
    connectionString ??= builder.Configuration.GetConnectionString("SqlServerConnection");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));
}
else
{
    connectionString ??= "Data Source=deadstockhair.db";
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(connectionString));
}

// Add database health check
builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>("database", tags: new[] { "ready", "db" });

// Add services with dependency injection
builder.Services.AddScoped<IRetailerService, RetailerService>();

// Keep the old RetailerStore for backwards compatibility if needed
// builder.Services.AddSingleton<RetailerStore>();

builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Initialize database
await app.InitializeDatabaseAsync();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors();

app.MapRetailerEndpoints();
app.MapSavedEndpoints();
app.MapScanEndpoints();

app.Run();
