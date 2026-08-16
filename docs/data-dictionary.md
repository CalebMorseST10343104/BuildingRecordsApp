# Data dictionary

This dictionary describes the current domain entities. `Required` refers to the intended/current storage model after all migrations are applied; it does not necessarily mean that missing information should block a partial record from being captured. The user-facing distinction between save-required, operationally important, and optional data is defined in [data-quality-rules.md](data-quality-rules.md).

## Property

| Field | Required | Meaning |
|---|---:|---|
| `PropertyId` | Yes | Internal generated identifier. |
| `Name` | Yes | Property or scheme name; unique in the register. |
| `Address` | Yes | Property-level address or description. |

## Building

| Field | Required | Meaning |
|---|---:|---|
| `BuildingId` | Yes | Internal generated identifier. |
| `PropertyId` | Yes | Property containing the building. |
| `Name` | Yes | Building name, unique within the property. |
| `Address` | Yes | Building-specific address text. |
| `NumberOfUnits` | Yes | Descriptive expected unit count; not derived from unit rows. |
| `NumberOfFloors` | Yes | Descriptive floor count. |

## Unit

| Field | Required | Meaning |
|---|---:|---|
| `UnitId` | Yes | Internal generated identifier. |
| `BuildingId` | Yes | Building containing the unit. |
| `UnitNumber` | Yes | Human-facing unit number, unique within the building. |
| `Bedrooms` | Yes | Bedroom count. |
| `DbInverter` | Yes | Whether a distribution-board inverter is recorded. |
| `Housekeeping` | Yes | Whether housekeeping-related information is recorded for the unit. The exact operational interpretation should be confirmed before expanding its use. |
| `PetFriendly` | Yes | Whether the owner permits pets in rentals; distinct from pets currently present. |
| `SublettingAllowed` | Yes | Whether subletting is allowed. |
| `AirconditioningUnits` | Yes | Number of air-conditioning units. |
| `PrimaryContactPersonId` | No | Preferred person to contact about the unit. |
| `AgentId` | No | Current letting agent. |

The entity still contains inverse/helper identifier properties such as `OwnershipId`, `LeaseId`, and `AccessDeviceCountId`. The authoritative relationships are the dependent records’ `UnitId` foreign keys; these helper properties should not be relied upon and are candidates for later cleanup.

## Person

| Field | Required in current schema | Meaning |
|---|---:|---|
| `PersonId` | Yes | Internal generated identifier. |
| `FirstName` | Yes | Given name. |
| `LastName` | Yes | Family name. |
| `Email` | Yes | Email address; may be blank when unknown in current data. |
| `PostalAddress` | Yes | Postal address; may be blank when unknown. |
| `IdNumber` | Yes | ID/passport reference; may be blank and is not unique. |
| `PhoneNumber` | Yes | Telephone number; may be blank when unknown. |

Business requirements allow incomplete person information, so the current non-null string representation uses empty strings. Converting these to genuinely nullable fields is a known cleanup item.

## Occupancy

| Field | Required | Meaning |
|---|---:|---|
| `OccupancyId` | Yes | Internal generated identifier. |
| `UnitId` | Yes | Occupied unit. |
| `OccupantId` | Yes | Person occupying the unit. |
| `OccupationType` | Yes | Descriptive category such as owner occupation or tenancy. Controlled values are not yet formally defined. |

## Ownership

| Field | Required | Meaning |
|---|---:|---|
| `OwnershipId` | Yes | Internal generated identifier. |
| `UnitId` | Yes | Unit to which the current ownership applies. |
| `OwnershipType` | Yes | `Natural` or `Juristic`. |
| `OrganizationId` | Conditional | Required for juristic ownership and prohibited for natural ownership. |

## OwnershipContact

| Field | Required | Meaning |
|---|---:|---|
| `OwnershipContactId` | Yes | Internal generated identifier. |
| `OwnershipId` | Yes | Ownership being represented. |
| `PersonId` | Yes | Person to contact in relation to that ownership. |

The current table, pages, routes, and code use `OwnershipContact`. Legacy `/Owners` addresses remain available as route aliases for old bookmarks.

## Organization

| Field | Required | Meaning |
|---|---:|---|
| `OrganizationId` | Yes | Internal generated identifier. |
| `Name` | Yes | Organization, company, or trust name. |
| `OrganizationType` | No | Descriptive type, where known. |
| `RegistrationReference` | No | Registration or trust reference; not assumed globally unique. |
| `Country` | No | Country or jurisdiction associated with registration. |
| `Address` | No | Organization address. |

The current table, pages, routes, and code use `Organization`. Legacy `/CompanyTrusts` addresses remain available as route aliases for old bookmarks.

## AgentCompany

| Field | Required | Meaning |
|---|---:|---|
| `AgentCompanyId` | Yes | Internal generated identifier. |
| `CompanyName` | Yes | Letting agency name. |
| `Address` | Yes | Agency address; may be blank if unknown. |
| `RegistrationNumber` | Yes | Agency registration reference. Uniqueness is not currently enforced. |

## Agent

| Field | Required | Meaning |
|---|---:|---|
| `AgentId` | Yes | Internal generated identifier for the role profile. |
| `PersonId` | Yes | Canonical person acting as agent; unique among agent profiles. |
| `AgentCompanyId` | Yes | Company employing the agent. |

Names, phone numbers, and email addresses come from the linked person record.

## Lease

| Field | Required | Meaning |
|---|---:|---|
| `LeaseId` | Yes | Internal generated identifier. |
| `UnitId` | Yes | Unit covered by the current lease summary. |
| `LeaseHolderName` | Yes | Free-text name appearing on the lease summary. |
| `StartDate` | Yes | Lease start date. |
| `EndDate` | Yes | Lease end date. |
| `DeclaredOccupantCount` | Yes | Number of occupants declared for the lease. |
| `SignedRules` | Yes | Whether conduct rules are recorded as signed. |
| `PetsPresent` | Yes | Whether pets are present as part of the current rental. |
| `EmergencyContactNumber` | Yes | Lease-specific emergency telephone number. |

## ParkingBay

| Field | Required | Meaning |
|---|---:|---|
| `ParkingBayId` | Yes | Internal generated identifier. |
| `PropertyId` | Yes | Property whose infrastructure contains the bay. |
| `ParkingBayNumber` | Yes | Bay number, unique within the property. |
| `IsNearEntrance` | Yes | Whether the bay is marked as near an entrance. |
| `UnitID` | No | Unit currently allocated the bay. The capitalized `ID` is a legacy naming inconsistency. |

## StoreRoom

| Field | Required | Meaning |
|---|---:|---|
| `StoreRoomId` | Yes | Internal generated identifier. |
| `PropertyId` | Yes | Property whose infrastructure contains the storeroom. |
| `StoreRoomNumber` | Yes | Storeroom number, unique within the property. |
| `UnitId` | No | Unit currently allocated the storeroom. |

## Vehicle

| Field | Required | Meaning |
|---|---:|---|
| `VehicleId` | Yes | Internal generated identifier. |
| `UnitId` | Yes | Unit with which the vehicle is currently associated. |
| `VehicleRegistration` | Yes | Vehicle registration, unique in the active register. |
| `VehicleModel` | Yes | Model description. |
| `VehicleMake` | Yes | Manufacturer/make description. |
| `VehicleColor` | Yes | Colour description. |

## AccessDeviceCount (Access Counts)

| Field | Required | Meaning |
|---|---:|---|
| `AccessDeviceCountId` | Yes | Internal generated identifier. |
| `UnitId` | Yes | Unit whose distribution counts are recorded; unique. |
| `OwnershipContactTagCount` | No | Gate tags held by ownership contacts. |
| `OwnershipContactRemoteCount` | No | Garage remotes held by ownership contacts. |
| `OccupantTagCount` | No | Gate tags held by occupants. |
| `OccupantRemoteCount` | No | Garage remotes held by occupants. |
| `AgentTagCount` | No | Gate tags held by the letting agent or agency. |
| `AgentRemoteCount` | No | Garage remotes held by the letting agent or agency. |

For every count field:

- `null` means the count is unknown;
- `0` means confirmed none;
- positive values are known quantities;
- negative values are invalid.
