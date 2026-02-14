var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.DeadStockHair_Api>("api");

builder.Build().Run();
