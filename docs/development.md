# Development guide

## Technology

- .NET 9
- ASP.NET Core Razor Pages
- Entity Framework Core 9
- SQLite
- AutoMapper
- xUnit with real in-memory SQLite databases

Core write operations are exposed through `UnitService`, `PropertyAllocationService`, `OwnershipService`, and `AgentService`. New UI and import code should use these services instead of reproducing their rules in page handlers.

## Build and run

From the `buildingapp` directory:

```bash
dotnet restore buildingapp.sln
dotnet build buildingapp.sln
dotnet run --project BuildingRecordsApp/BuildingRecordsApp.csproj
```

The SQLite connection is configured under `ConnectionStrings:BuildingContext`. Foreign-key enforcement is enabled in the connection string. Relative SQLite paths are resolved against the application's content root, so launching the app from a different working directory does not silently select a different database.

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
- legacy-to-current migration behavior;
- completeness and dashboard rules;
- database-error translation;
- CRUD form conventions;
- real Razor Page request journeys.

The Razor Page integration tests run the actual application with an isolated temporary SQLite database. They exercise antiforgery-protected form posts, validation redisplay, dashboard filtering, issue resolution, and every current index and create surface. Never point these tests at the development or deployed database; database replacement in `BuildingRecordsWebApplicationFactory` is deliberately explicit and covered by the journey tests.

## Register index conventions

Register index pages use the shared `InfoDisplay` component wherever their layout permits it. Each displayed column has its own text filter, and relationship context such as Property, Building, and Unit should be included where it helps distinguish otherwise similar records.

User-facing headings come from `Display(Name = "...")` attributes on item view-model properties. This keeps database and C# property names stable while allowing natural labels in the interface. When no attribute is present, the component separates a PascalCase property name into words as a fallback.

Record actions appear alongside each record on its right. The action cell is removed from the table layout and placed in reserved space beyond the table's right border, so the controls align with the record without looking like another data column. Use Open only when the record has a useful details page, use Edit and Delete for ordinary editable records, and omit actions that conflict with a business rule. Access device counts are therefore Edit-only because their one-to-one records must exist for every unit and cannot be deleted independently.

Properties and access device counts retain custom layouts, but must expose equivalent filters, readable labels, separated actions, and clear empty/no-match states.

## Excel register exports

The Export page creates a current-state `.xlsx` workbook for either a whole property or selected buildings within that property. No export operation changes the database. The resulting file contains personal and contact information and must be treated with the same care as a database backup.

`RegisterExportService` owns scope validation and database projection. Building IDs supplied by the browser are accepted only when they belong to the selected property. With no building IDs selected, all buildings in the property are included. Because parking bays and storerooms belong to a property rather than a building, a whole-property export includes allocated and unallocated infrastructure; a partial-building export includes infrastructure currently allocated to units in the selected buildings.

`RegisterExcelWorkbookWriter` writes the workbook without formulas or macros. Values are stored as typed numbers, dates, or text, and text is XML-escaped so register content cannot become an Excel formula. The expected worksheets are MAIN, PEOPLE, NATURAL OWNERSHIP, JURISTIC OWNERSHIP, AGENTS, OCCUPANTS, LEASES, VEHICLES, TAG AND REMOTE, and BASEMENT. MAIN is one row per unit and uses colour-banded sections modelled on the original collection register; the other sheets retain normalized one-record-per-row structures.

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

- An existing database is backed up automatically before pending startup migrations are applied.
- Manual, downloadable backups are available from `/Backups`.
- `DatabaseBackups:Directory` controls the backup directory; relative paths resolve against the application content root.
- `DatabaseBackups:RetainedBackupCount` controls automatic pruning and defaults to 30.
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

## Current terminology

Use `Organization`, `OwnershipContact`, and `AccessDeviceCount` consistently in new code, pages, and database objects. The old `/CompanyTrusts`, `/Owners`, and `/TagRemoteRecords` URLs are route aliases only, retained so existing bookmarks do not break.

## Current-state design

The application does not maintain temporal history. Do not add `ValidFrom`, `ValidTo`, soft-deletion, or audit-history behavior casually: that would be a material product decision affecting imports, screens, queries, and privacy obligations.

Backups and exports are the current history mechanism.

## Friendly database errors

All environments use the `/Error` page, including development, so raw exception pages and database messages are never shown in the browser. The original exception and request identifier are still written to the application log for diagnosis.

`DatabaseErrorTranslator` converts database failures into safe, actionable messages. It recognizes SQLite error categories such as duplicate values, records that are still in use, missing or invalid values, concurrent changes, and a busy or unavailable database. Known unique constraints also have specific fingerprints so, for example, a duplicate vehicle registration receives a more useful message than a generic conflict.

When adding a unique database rule:

1. Give the rule a normal Entity Framework unique index or constraint.
2. Add its SQLite column fingerprint and user-facing message to `DatabaseErrorTranslator`.
3. Add a translator test proving the technical constraint message cannot reach the user.

Pre-save validation should still be used where it improves the form experience. The centralized translator is the final safety net for race conditions, legacy data, and unexpected database failures.

## CRUD form conventions

Create and edit pages use a shared `_Form.cshtml` partial within each record folder. Keep these pages predictable:

- render the partial with `<partial name="_Form" for="ViewModel" />` so posted field names match the bound page model;
- put the model-level validation summary and the hidden edit identifier in the shared partial;
- use Bootstrap `form-label`, `form-control`, `form-select`, and `form-check` classes consistently;
- use `Save new record` on create pages, `Save changes` on edit pages, and `Cancel` for the non-destructive return action;
- reload every dropdown list before returning the page after server-side validation fails;
- include `_ValidationScriptsPartial` so field validation works before submission as well as on the server.

The CRUD form convention tests inspect every current record folder. Add a new record type to that test list when introducing another CRUD surface.
