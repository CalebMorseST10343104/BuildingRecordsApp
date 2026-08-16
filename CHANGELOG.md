# Changelog

This project uses [Semantic Versioning](https://semver.org/): `MAJOR.MINOR.PATCH`.

## 1.1.0 — 2026-08-16

- Generalized the product identity from Chelsea Building Register to Building Manager.
- Replaced Chelsea-specific defaults and documentation with neutral examples.
- Added automatic migration of the original Mac application data directory during an upgrade.

## 1.0.0 — 2026-08-16

Initial single-user release of the building register application.

- Normalized property, building, unit, person, ownership, occupancy, letting, vehicle, parking, storeroom, lease, and access-device records.
- Added unit-centred register workflows and standardized validation and friendly error handling.
- Added operational-completeness checks and the follow-up dashboard.
- Added database backups and safe Intel macOS desktop packaging.
- Added filtered register indexes and multi-sheet Excel exports.
- Added automated database, service, validation, and Razor Page integration tests.
