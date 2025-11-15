# NotesApp

This is boilerplate repository about implementing Microservices with Aspire 13 and .NET 10. It contains sample microservices and demonstrates several new .NET 10 features, as well as integrations, that can act as a solid starter for future projects.

## Projects
- `NotesApi` - a REST service for notes.
- `TagsApi` - a REST service for tags related to notes.
- `NotesApp.AppHost` - local apphost that wires up the services with helper containers and endpoints.
- `NotesApp.ServiceDefaults` - exposes shared helper extension methods to configure common services (OpenTelemetry, health checks, resilience etc.)

## Requirements
- .NET 10 SDK
- Aspire 13 NuGet feed access (see https://docs.aspire.dev/getting-started/nuget/)

## Central package management
This solution uses the `Directory.Packages.props` file and Central Package Management (CPM). That means package versions are declared centrally and projects omit version numbers. This helps avoid version drift across microservices.

Notes on EF version: To remain compatible with the `Aspire.Npgsql.EntityFrameworkCore.PostgreSQL` (the version used here), Entity Framework Core packages are pinned to the 9.x series (`Microsoft.EntityFrameworkCore 9.0.9`). If you want to upgrade to EF 10 in the future, you'll need to verify Aspire & Npgsql package versions that support EF 10 and update `Directory.Packages.props` accordingly.

## Getting started - (build, run, migrations)
1. Restore packages:

```pwsh
cd .\NotesApp
dotnet restore
```

2. Build the whole solution:

```pwsh
dotnet build -clp:Summary
```

3. Run migrations

- We've added design-time factories that read `appsettings.json` (and `appsettings.Development.json`) to obtain the DB connection string for migrations. Ensure your connection string is valid.

- From `TagsApi` project folder (recommended):

```pwsh
cd .\TagsApi
# Make sure your connection string is valid
dotnet ef migrations add InitialCreate -o Data/Migrations
dotnet ef database update
```

- From `NotesApi` project folder (recommended):

```pwsh
cd .\NotesApi
dotnet ef migrations add InitialCreate -o Data/Migrations
dotnet ef database update
```

Design-time factories were added for both `TagsDbContext` and `NotesDbContext` to ensure `dotnet ef` can construct a working DbContext even when the app host isn't used.


## How to run via AppHost
The `NotesApp.AppHost` project helps running services together locally, including other dependencies like Postgres and Redis. Use it when you want a local integration / dev environment:

```pwsh
cd .\NotesApp.AppHost
dotnet run
```

The apphost will wire up `tags-api` and `notes-api` and required dependencies.

## Quality & conventions
- Central package management is enforced by `Directory.Packages.props` (`ManagePackageVersionsCentrally=true`). If you add a PackageReference in a project, omit the `Version` attribute unless you intend to override central versions.
- EF Core version is pinned to avoid conflicts with transitive packages from the Aspire library.

## Helpful commands
- Restore & build the solution
  - dotnet restore
  - dotnet build -clp:Summary
- Add migrations
  - cd TagsApi
  - dotnet ef migrations add InitialCreate -o Data/Migrations
- Apply migrations
  - dotnet ef database update

## Next steps / Improvements
- Add GitHub Actions or another CI flow to run `dotnet restore`, `dotnet build`, and `dotnet ef migrations` as part of validation.
- Add End-to-end tests that exercise `AddServiceDefaults` and default endpoints to avoid regression.
- Implement a secure local development strategy for DB and credentials (secrets manager or developer `.env` file) instead of using plaintext in code.

---


