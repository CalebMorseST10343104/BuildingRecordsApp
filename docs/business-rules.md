# Business rules

This is the authoritative catalogue of currently agreed register behavior.

Field-level save requirements and operational incompleteness are specified in [data-quality-rules.md](data-quality-rules.md).

## Enforcement labels

| Label | Meaning |
|---|---|
| **Database** | Enforced by keys, foreign keys, unique indexes, check constraints, or deletion behavior. |
| **Application** | Enforced in the current Razor Pages workflow but not necessarily by direct database access. |
| **Policy** | Agreed behavior that is not yet completely enforced by software. |
| **Open** | The business meaning or desired implementation still needs a decision. |

## Register-wide rules

| Rule | Enforcement |
|---|---|
| The register stores current state rather than ownership, lease, occupation, or allocation history. | Policy |
| Historical snapshots are currently handled through backups or exports. | Policy |
| A record may be incomplete where explicitly allowed below; missing information should not be fabricated. | Policy |

## Property and physical structure

| Rule | Enforcement |
|---|---|
| A building belongs to exactly one property. | Database |
| A unit belongs to exactly one building. | Database |
| A unit number is unique within its building, but may repeat in another building. | Database, tested |
| A property name is unique in the register. | Database |
| A building name is unique within its property. | Database |
| A parking-bay number is unique within its property. | Database, tested |
| A storeroom number is unique within its property. | Database, tested |
| A parking bay or storeroom may be unallocated or allocated to one unit. | Database |
| A bay or storeroom may only be allocated to a unit in the same property. | Application service and current create/edit workflows; direct database writes can bypass it |
| Deleting a property that still contains buildings, bays, or storerooms is prohibited. | Database |
| Deleting a building that still contains units is prohibited. | Database |

## People and contact roles

| Rule | Enforcement |
|---|---|
| Personal details are stored once in a canonical person record. | Model design |
| A unit may have zero or one primary contact. | Database |
| A person may be primary contact for several units. | Database |
| Deleting a primary-contact person clears the unit’s primary-contact assignment. | Database |
| ID/passport values are optional in business terms and are not treated as globally unique identifiers. | Policy; the current field type still needs UI/data cleanup for truly optional storage |

## Occupancy

| Rule | Enforcement |
|---|---|
| One occupancy row represents one person-unit relationship, not a household. | Model design |
| An occupancy must reference one person and one unit. | Database |
| A person may occupy several units simultaneously. | Database, tested |
| A unit may have no occupants or several occupants. | Database |
| The same person-unit pair cannot be entered twice. | Database, tested |
| Deleting a person or unit deletes its occupancy relationships. | Database, tested for unit deletion |

## Ownership

| Rule | Enforcement |
|---|---|
| Every unit should have exactly one current ownership record. | Policy; the database prevents more than one but cannot require the child row’s existence |
| An ownership belongs to exactly one unit. | Database |
| Ownership type is `Natural` or `Juristic`. | Database indirectly through the current type/organization check |
| Natural ownership must not reference an organization. | Database, tested |
| Juristic ownership must reference an organization. | Database, tested |
| An ownership may temporarily have no ownership contacts. | Database |
| An ownership may have several contacts. | Database, tested |
| A person may be a contact for several ownerships. | Database |
| The same person cannot be attached twice to the same ownership. | Database, tested |
| Organization registration references are not automatically treated as unique. | Policy |
| Deleting a unit deletes its ownership and ownership-contact relationships. | Database, tested |

## Letting agents

| Rule | Enforcement |
|---|---|
| An agent profile references exactly one canonical person. | Database |
| A person can have at most one agent profile. | Database, tested |
| An agent belongs to exactly one agent company. | Database |
| Freelance agents are not currently supported. | Model design |
| A unit may have zero or one letting agent. | Database |
| An agent may manage several units, including none temporarily. | Database |
| Deleting an agent clears its unit assignments but does not delete the person. | Database |

## Leases and pets

| Rule | Enforcement |
|---|---|
| A lease summary belongs to exactly one unit. | Database |
| A unit may have zero or one current lease summary. | Database, tested |
| Lease-holder name is intentionally free text. | Model design |
| Declared occupant count does not replace individual occupancy records. | Policy |
| `Lease.PetsPresent` records whether pets are present in the current rental. | Policy/model naming |
| `Unit.PetFriendly` records whether pets are permitted by the owner for rentals. | Policy/model naming |
| Deleting a unit deletes its lease summary. | Database, tested |
| Whether a lease may have several named signatories remains undecided. | Open |

## Vehicles

| Rule | Enforcement |
|---|---|
| A vehicle record belongs to exactly one unit. | Database |
| A vehicle registration is unique in the current register. | Database, tested |
| Vehicles that are no longer relevant should be deleted rather than retained as history. | Policy |
| Deleting a unit deletes its vehicles. | Database, tested |

## Access-device counts

| Rule | Enforcement |
|---|---|
| Access devices are represented as counts, not individually identified assets. | Model design |
| Every unit should have exactly one access-count record. | Transactional unit service creates one; database prevents duplicates but cannot require existence |
| An access-count record cannot be deleted independently through the current UI. | Application |
| Deleting a unit deletes its access-count record. | Database, tested |
| Counts may be null, meaning unknown. | Database, tested |
| Zero means confirmed none. | Policy, tested as storable |
| Counts cannot be negative. | Database, tested |

## Known enforcement gaps

These are useful candidates for subsequent implementation and tests:

1. Decide how and when a newly created unit receives its initially incomplete ownership record; access-count creation is already transactional.
2. Prevent independent access-count deletion outside the current Razor Page handler.
3. Make genuinely optional person fields nullable consistently in the database, forms, and imports.
4. Apply the documented data-quality rules consistently across every CRUD form and translate database failures into friendly validation messages.
5. Add the calculated incomplete-record dashboard defined by the data-quality specification.
