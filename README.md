# Building Records App

Building Records App is a .NET Razor Pages application for maintaining the current operational register of a sectional-title property. It replaces a wide, repetitive spreadsheet with structured records for units, people, ownership, occupation, letting agents, leases, vehicles, parking, storerooms, and access-device counts.

The initial deployment target is Chelsea, a property containing two buildings with some shared infrastructure. The model supports more than one property, but the current user interface is still oriented around the initial Chelsea property.

## Project status

The project is being revived and incrementally modernized. The domain model and core database constraints have been normalized. Automated database and migration tests are being introduced. This is not yet a production-ready HOA management system, and the checked-in database must not be treated as an authoritative live register.

## Documentation

- [Domain model](docs/domain-model.md) — the real-world concepts and how they relate.
- [Business rules](docs/business-rules.md) — required behavior and its current enforcement status.
- [Data dictionary](docs/data-dictionary.md) — the meaning of entities and fields.
- [Data validity and operational completeness](docs/data-quality-rules.md) — what blocks saving, what requires follow-up, and what is genuinely optional.
- [Development guide](docs/development.md) — setup, migrations, tests, and repository conventions.
- [Intel macOS deployment](docs/macos-deployment.md) — packaging, installation, upgrades, and recovery for the initial single-user deployment.
- [Releasing and packaging](docs/releasing.md) — semantic versioning and the repeatable self-service release workflow.

## Solution structure

```text
buildingapp/
├── BuildingRecordsApp/         # ASP.NET Core Razor Pages application
├── BuildingRecordsApp.Tests/   # xUnit and SQLite tests
├── docs/                       # Domain and development documentation
└── buildingapp.sln
```

## Quick start

Requirements:

- .NET 9 SDK
- SQLite

From this directory:

```bash
dotnet restore buildingapp.sln
dotnet run --project BuildingRecordsApp/BuildingRecordsApp.csproj
```

Run the automated tests with:

```bash
dotnet test buildingapp.sln
```

The application applies pending Entity Framework migrations at startup. When an existing database needs migration, the application first creates an integrity-checked SQLite backup. Manual backups can also be created and downloaded from the **Backups** page.

The current register can be downloaded as a multi-sheet Excel workbook from the **Export** page. Select a property and optionally one or more of its buildings; leaving every building unticked exports the complete property.

The default database is `BuildingRecordsApp/buildingrecords.db`, resolved relative to the application content directory rather than the shell's working directory. Both the database connection and backup directory can be overridden through configuration.

## Scope

The register represents **current state**, not an audit history. Reassignments and replacements update the current records. If historical snapshots are required, they should currently be created as database or exported-register backups.

Potential later modules include short-term-rental approval, document generation, WhatsApp message generation, surveys, billing assistance, and operational queries. Those modules are not part of the current register model unless explicitly documented.
