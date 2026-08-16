# Data validity and operational completeness

This document defines what the register must reject, what it may temporarily accept as incomplete, and what is genuinely optional. It is the implementation specification for form validation, friendly CRUD errors, and the incomplete-record dashboard.

The [business rules](business-rules.md) remain authoritative for entity relationships and current-state behavior. The [data dictionary](data-dictionary.md) describes storage fields. This document governs data quality from the user's perspective.

## Quality classifications

### Save-required

A save-required value or relationship is necessary for the record to have a stable identity or valid relationship. A create or edit request that omits it must remain on the form and show a field-specific message. It must not reach a raw database error page.

### Conditionally required

A conditionally required value becomes save-required because of another submitted value. For example, juristic ownership requires an organization, while natural ownership prohibits one.

### Operationally important

The database may contain the record without this information so that partially known data can be captured. Its absence creates an item on the incomplete-record dashboard. Missing information must never be fabricated merely to clear an issue.

### Optional

The absence of the information is valid and does not create an incomplete-record issue. Optional does not mean ignored: when supplied, the value must still have a valid format and refer to an existing record where applicable.

## Issue severity

| Severity | Meaning | Examples |
|---|---|---|
| **Urgent** | The omission prevents reliable ownership, contact, access-control, or current-lease administration. | Unit has no ownership; contact role has no phone or email; lease has expired. |
| **Important** | The record is usable, but information normally needed for administration is unknown. | Unknown access-device count; missing vehicle description; missing building address. |
| **Informational** | Worth completing when convenient, but not part of the initial dashboard unless explicitly enabled later. | Organization country; a person's postal address when the person has no active contact role. |

Severity affects display and sorting only. It does not change whether a record may be saved.

## Register-wide validity rules

- Text used as a required name, number, or registration must contain at least one non-whitespace character.
- Leading and trailing whitespace is removed before comparison and storage.
- Foreign-key selections must identify records that still exist. A stale or fabricated identifier produces a friendly field error.
- A value protected by a uniqueness rule must be checked before saving; the database constraint remains the final safeguard against races.
- Numeric counts cannot be negative.
- If an optional email address, telephone number, or date is supplied, its value must be valid.
- `null` or blank means unknown only where this document permits incomplete information. It must not silently mean zero, false, or “not applicable.”
- Boolean fields currently always contain `true` or `false`; neither value is considered missing.
- A failure caused by concurrency, a database constraint, or a record being deleted by another request must produce a friendly message and preserve the submitted form where possible.

## Entity rules

### Property

Save-required:

- Name.
- Name must be unique across properties.

Operationally important:

- Address or property description — **Important**.

Optional:

- Having no buildings yet is valid.

### Building

Save-required:

- Existing property.
- Name.
- Name must be unique within the property.
- Unit and floor counts, when supplied, cannot be negative.

Operationally important:

- Address — **Important**.
- Expected unit count not known or recorded as zero — **Important**.
- Floor count not known or recorded as zero — **Important**.

Optional:

- A building may temporarily have no unit records.

The current non-null integer storage cannot distinguish an unknown count from a confirmed zero. Until that storage is improved, zero is treated as incomplete for buildings because an instantiated residential building is expected to contain at least one floor and unit.

### Unit

Save-required:

- Existing building.
- Unit number.
- Unit number must be unique within the building.
- Bedroom and air-conditioning counts cannot be negative.

Operationally important:

- Primary contact assignment — **Urgent**.
- Current ownership record — **Urgent**.
- Access-device count record — **Urgent**.

Optional:

- Letting agent.
- Lease.
- Occupants.
- Vehicles.
- Parking bays.
- Storerooms.
- A bedroom count of zero is valid for a studio unit.
- An air-conditioning count of zero means confirmed none.
- `DbInverter`, `Housekeeping`, `PetFriendly`, and `SublettingAllowed` are known Boolean answers; `false` is valid and not incomplete.

### Person

Save-required:

- First name.
- Last name.
- Email and telephone formats must be valid when supplied.

Operationally important:

- A person assigned as a unit primary contact, ownership contact, or letting agent must have at least one of phone number or email address — **Urgent**.
- Legacy person record with a blank first or last name — **Urgent**.

Optional:

- Email when a usable phone number exists.
- Phone number when a usable email exists.
- Postal address.
- ID or passport reference.
- Having neither phone nor email when the person has no active contact role.

ID and passport values are not assumed globally unique. Missing contactability is evaluated from the canonical person once, regardless of how many contact roles that person fills.

### Organization

Save-required:

- Name.

Operationally important when linked to a current juristic ownership:

- Registration reference — **Important**.
- Address — **Important**.

Optional:

- Organization type.
- Country or jurisdiction.
- Registration reference and address while the organization is not attached to a current ownership.

Registration references are not database-unique because companies, trusts, jurisdictions, and foreign registration systems do not share a single comparable format.

### Ownership

Save-required:

- Existing unit.
- Type must be exactly `Natural` or `Juristic`.
- Only one current ownership may exist for a unit.

Conditionally required:

- Juristic ownership requires an existing organization.
- Natural ownership must not reference an organization.

Operationally important:

- At least one ownership contact — **Urgent**.

Optional:

- Several ownership contacts are valid.
- A contact may represent several ownerships.

### Ownership contact

Save-required:

- Existing ownership.
- Existing person.
- The same person/ownership pair cannot be added twice.

Operationally important:

- Contactability is evaluated on the linked person. Neither phone nor email creates an **Urgent** issue.

Optional:

- No separate role or job title inside the organization is required.

### Agent company

Save-required:

- Company name.

Operationally important when the company has a current agent profile:

- Address — **Important**.
- Registration number — **Important**.

Optional:

- Address and registration number while no current agent is attached.
- Registration numbers are not currently required to be unique.

### Agent

Save-required:

- Existing canonical person.
- Existing agent company.
- A person may have only one agent profile.

Operationally important:

- Contactability is evaluated on the linked person. Neither phone nor email creates an **Urgent** issue.
- Company completeness is evaluated through the linked agent company.

Optional:

- An agent may currently manage no units.

Freelance agents are not supported.

### Occupancy

Save-required:

- Existing unit.
- Existing person.
- The same person/unit pair cannot be entered twice.

Operationally important:

- Occupation type or category — **Important**.

Optional:

- A person may occupy several units.
- A unit may have no occupants.
- Occupant phone, email, and postal address are not automatically required unless the person also fills a contact role.

The allowed occupation-type vocabulary remains open. Until controlled values are agreed, a non-blank descriptive value is sufficient to resolve the issue.

### Lease

The absence of a lease record is valid. If a lease record exists, the following rules apply.

Save-required:

- Existing unit.
- Only one current lease summary per unit.
- Lease-holder name.
- Start date.
- End date.
- End date must not precede start date.
- Declared occupant count must be zero or greater.

Operationally important:

- Emergency contact number — **Important**.
- End date earlier than the current date — **Urgent**, because the current-state lease summary must be reviewed, replaced, or removed.

Optional:

- A declared occupant count of zero is valid.
- Signed-rules `false` is a valid recorded answer, not missing data.
- Pets-present `false` is a valid recorded answer, not missing data.
- Individual occupancy rows may differ temporarily from the declared occupant count; this is not initially treated as an error.

### Parking bay

Save-required:

- Existing property.
- Bay number.
- Bay number must be unique within the property.
- If allocated, the selected unit must exist in the same property.

Optional:

- Unit allocation.
- Near-entrance `false` is a valid recorded answer.

An unallocated parking bay is not incomplete because bays are permanent property infrastructure and can exist between allocations.

### Storeroom

Save-required:

- Existing property.
- Storeroom number.
- Storeroom number must be unique within the property.
- If allocated, the selected unit must exist in the same property.

Optional:

- Unit allocation.

An unallocated storeroom is not incomplete because storerooms can exist between allocations.

### Vehicle

Save-required:

- Existing unit.
- Registration number.
- Registration number must be unique in the current register.

Operationally important:

- Make — **Important**.
- Model — **Important**.
- Colour — **Important**.

Optional:

- A unit may have no vehicle records.

Vehicles that are no longer relevant are deleted rather than retained as history.

### Access-device count

Save-required:

- Existing unit.
- Only one access-device count record per unit.
- Every supplied count must be zero or greater.

Operationally important:

- Each unknown tag or remote count — **Important**.
- A unit with no access-device count record — **Urgent** at the unit level.

Interpretation:

- `null` means unknown and creates an issue.
- `0` means confirmed none and is complete.
- A positive integer is a known quantity and is complete.

The six ownership-contact, occupant, and agent counts remain relevant even when the corresponding role is not currently assigned. Zero should be recorded when confirmed none; the dashboard must not infer zero from the absence of a person or agent relationship.

## Cross-record rules

These rules require evaluating more than one entity:

| Rule | Result when unmet |
|---|---|
| Every unit has one current ownership. | Urgent unit issue. |
| Every ownership has at least one ownership contact. | Urgent ownership issue. |
| Every unit has one access-device count record. | Urgent unit issue. |
| Every active primary contact, ownership contact, and agent is contactable by phone or email. | Urgent person issue, shown once with affected roles/units. |
| Juristic ownership has an organization with registration reference and address. | Important organization issues. |
| Agent profile has a company with registration number and address. | Important company issues. |
| Current lease end date has not passed. | Urgent lease-review issue. |
| Parking and storeroom allocations remain within one property. | Invalid save; not an incompleteness issue. |

## Dashboard behavior implied by these rules

- Issues are calculated from current records; no separate resolved/incomplete flag is stored.
- Correcting the underlying data removes the issue automatically.
- Multiple missing fields on one record are grouped into one dashboard row where practical.
- The default view sorts urgent issues before important issues and groups unit-related work by property, building, and unit.
- Each issue provides a direct link to the relevant edit or relationship-creation page.
- Optional omissions never appear.
- The dashboard describes unknown data; it does not mutate records or invent defaults.

## Implementation status

This document records the approved target behavior from phase one. Phase two implements the urgent and important issue calculations in `RegisterCompletenessService`, including correction URLs and property/building/unit context. The dashboard UI and CRUD hardening remain subsequent phases, so the current forms may still accept some save-invalid data and there is not yet a user-facing issue list.
