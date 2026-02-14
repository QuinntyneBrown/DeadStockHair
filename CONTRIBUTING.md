# Contributing

Thanks for your interest in contributing to DeadStockHair CLI.

## Development Setup

1. Install the [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0).
2. Clone the repository and build:

```bash
git clone https://github.com/your-username/DeadStockHair.git
cd DeadStockHair
dotnet build src/DeadStockHair.Cli
```

3. Install Playwright browsers:

```bash
pwsh src/DeadStockHair.Cli/bin/Debug/net9.0/playwright.ps1 install
```

## Making Changes

1. Create a branch from `main`:

```bash
git checkout -b feature/your-feature-name
```

2. Make your changes and verify the project builds:

```bash
dotnet build src/DeadStockHair.Cli
```

3. Run the tool to manually verify your changes:

```bash
dotnet run --project src/DeadStockHair.Cli -- scan
```

## Project Conventions

- **Target framework:** .NET 9.0
- **Nullable reference types:** Enabled project-wide
- **Dependency injection:** Register services in `Program.cs` via `Host.CreateDefaultBuilder`
- **Commands:** Add new commands under `Commands/` as static classes with a `Create(IServiceProvider)` method
- **Models:** Use C# records for data models under `Models/`
- **Services:** Define an interface in `Services/` and register the implementation in `Program.cs`

## Pull Requests

- Keep PRs focused on a single change.
- Write a clear description of what the PR does and why.
- Make sure the project builds with no warnings before submitting.

## Reporting Issues

Open an issue on GitHub with:

- A clear title and description.
- Steps to reproduce the problem.
- Expected vs actual behavior.
- .NET SDK version (`dotnet --version`).
