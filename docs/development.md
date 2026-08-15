# Development guide

## Technology

- .NET 9
- ASP.NET Core Razor Pages
- Entity Framework Core 9
- SQLite
- AutoMapper
- xUnit with real in-memory SQLite databases

## Build and run

From the `buildingapp` directory:

```bash
dotnet restore buildingapp.sln
dotnet build buildingapp.sln
dotnet run --project BuildingRecordsApp/BuildingRecordsApp.csproj
```

The SQLite connection is configured under `ConnectionStrings:BuildingContext`. Foreign-key enforcement is enabled in the connection string.

## Tests

Run the complete suite with:

```bash
dotnet test buildingapp.sln
```

The database tests use SQLite rather than Entity Framework’s in-memory provider. This is intentional: SQLite exercises the unique indexes, foreign keys, check constraints, cascade behavior, and migrations used by the application.

Current test categories include:

- uniqueness constraints;
- relationship cardinality;
- ownership-type rules;
- access-count semantics;
- cascade deletion;
- legacy-to-current migration behavior.

When adding a business rule:

1. Add or update the rule in [business-rules.md](business-rules.md).
2. Decide whether it belongs in the database, a reusable application service, or both.
3. Add an automated test at the same time as the implementation.
4. Update the [data dictionary](data-dictionary.md) if field meaning or optionality changes.

## Database migrations

Create a migration after changing the Entity Framework model:

```bash
dotnet ef migrations add DescriptiveMigrationName \
  --project BuildingRecordsApp/BuildingRecordsApp.csproj \
  --startup-project BuildingRecordsApp/BuildingRecordsApp.csproj
```

Review every generated migration before applying it. Entity Framework can infer an incorrect rename or introduce a default such as `0` for an existing required foreign key. Data-preserving SQL may be needed before a nullable relationship becomes required.

Apply migrations explicitly with:

```bash
dotnet ef database update \
  --project BuildingRecordsApp/BuildingRecordsApp.csproj \
  --startup-project BuildingRecordsApp/BuildingRecordsApp.csproj
```

The application also calls `Database.Migrate()` at startup.

### Database safety

- Back up a meaningful register before applying migrations.
- Test migrations on a copied database first.
- Do not use the checked-in SQLite file as the only copy of live data.
- Do not commit incidental local database changes with source-code changes.
- Never replace `Migrate()` with `EnsureCreated()` for an existing database; `EnsureCreated()` bypasses migration history.

## Repository conventions

- `bin/`, `obj/`, `.DS_Store`, and other generated artifacts must remain untracked.
- Keep domain entities under `Models/Entities`.
- Keep Entity Framework relationship and constraint configuration in `BuildingContext`.
- Use form view models for user input and item view models for display.
- Prefer canonical `Person` records over copying personal details into role entities.
- Use generated integer primary keys. Treat human identifiers such as unit numbers and registrations as constrained business values, not primary keys.
- Preserve existing user data when writing migrations.

## Current naming compatibility

The normalized domain uses `Organization` and `OwnershipContact`. Some database table names, Razor Page folders, routes, view-model class names, and `DbSet` property names still use the historical terms `CompanyTrust` and `Owner`.

This compatibility is intentional for the current migration slice. New code should use the normalized domain terminology unless it must interact with one of those compatibility surfaces. A future cleanup can rename routes and tables through an explicit, tested migration.

## Current-state design

The application does not maintain temporal history. Do not add `ValidFrom`, `ValidTo`, soft-deletion, or audit-history behavior casually: that would be a material product decision affecting imports, screens, queries, and privacy obligations.

Backups and exports are the current history mechanism.
