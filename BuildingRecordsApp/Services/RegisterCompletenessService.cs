using BuildingRecordsApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BuildingRecordsApp.Services;

public sealed class RegisterCompletenessService(BuildingContext context, TimeProvider timeProvider)
    : IRegisterCompletenessService
{
    public async Task<IReadOnlyList<CompletenessIssue>> GetIssuesAsync(CancellationToken cancellationToken = default)
    {
        var issues = new List<CompletenessIssue>();

        var properties = await context.Properties.AsNoTracking().ToListAsync(cancellationToken);
        foreach (var property in properties)
        {
            if (Blank(property.Name))
                issues.Add(Issue("property.name.missing", CompletenessSeverity.Urgent, RegisterRecordType.Property,
                    property.PropertyId, PropertyLabel(property), "Property name is missing.", $"/Properties/Edit?id={property.PropertyId}",
                    property.PropertyId, property.Name));
            if (Blank(property.Address))
                issues.Add(Issue("property.address.missing", CompletenessSeverity.Important, RegisterRecordType.Property,
                    property.PropertyId, PropertyLabel(property), "Property address or description is missing.", $"/Properties/Edit?id={property.PropertyId}",
                    property.PropertyId, property.Name));
        }

        var buildings = await context.Buildings.Include(b => b.Property).AsNoTracking().ToListAsync(cancellationToken);
        foreach (var building in buildings)
        {
            AddBuildingIssueIf(Blank(building.Name), "building.name.missing", CompletenessSeverity.Urgent, building, "Building name is missing.");
            AddBuildingIssueIf(Blank(building.Address), "building.address.missing", CompletenessSeverity.Important, building, "Building address is missing.");
            AddBuildingIssueIf(building.NumberOfUnits <= 0, "building.expected-units.unknown", CompletenessSeverity.Important, building, "Expected unit count is unknown or zero.");
            AddBuildingIssueIf(building.NumberOfFloors <= 0, "building.floors.unknown", CompletenessSeverity.Important, building, "Floor count is unknown or zero.");
        }

        var units = await context.Units
            .Include(u => u.Building).ThenInclude(b => b!.Property)
            .Include(u => u.Ownership)
            .Include(u => u.AccessDeviceCount)
            .AsNoTracking().AsSplitQuery().ToListAsync(cancellationToken);
        foreach (var unit in units)
        {
            AddUnitIssueIf(Blank(unit.UnitNumber), "unit.number.missing", CompletenessSeverity.Urgent, unit, "Unit number is missing.", $"/Units/Edit?id={unit.UnitId}");
            AddUnitIssueIf(unit.PrimaryContactPersonId is null, "unit.primary-contact.missing", CompletenessSeverity.Urgent, unit, "No primary contact is assigned.", $"/Units/Edit?id={unit.UnitId}");
            AddUnitIssueIf(unit.Ownership is null, "unit.ownership.missing", CompletenessSeverity.Urgent, unit, "No current ownership is recorded.", $"/Ownerships/Create?unitId={unit.UnitId}");
            AddUnitIssueIf(unit.AccessDeviceCount is null, "unit.access-count.missing", CompletenessSeverity.Urgent, unit, "The access-device count record is missing.", $"/Units/Details?id={unit.UnitId}");
        }

        var ownerships = await context.Ownerships
            .Include(o => o.Unit)!.ThenInclude(u => u!.Building)!.ThenInclude(b => b!.Property)
            .Include(o => o.OwnershipContacts)
            .AsNoTracking().AsSplitQuery().ToListAsync(cancellationToken);
        foreach (var ownership in ownerships)
        {
            if (ownership.OwnershipContacts.Count == 0 && ownership.Unit is not null)
                issues.Add(UnitIssue("ownership.contacts.missing", CompletenessSeverity.Urgent, RegisterRecordType.Ownership,
                    ownership.OwnershipId, $"Ownership for {UnitLabel(ownership.Unit)}", "No ownership contact is recorded.",
                    $"/OwnershipContacts/Create?ownershipId={ownership.OwnershipId}", ownership.Unit));
        }

        var people = await context.Persons
            .Include(p => p.PrimaryContactUnits).ThenInclude(u => u.Building)!.ThenInclude(b => b!.Property)
            .Include(p => p.OwnershipContacts).ThenInclude(c => c.Ownership).ThenInclude(o => o.Unit)!.ThenInclude(u => u!.Building)!.ThenInclude(b => b!.Property)
            .Include(p => p.AgentProfile)!.ThenInclude(a => a!.Units).ThenInclude(u => u.Building)!.ThenInclude(b => b!.Property)
            .AsNoTracking().AsSplitQuery().ToListAsync(cancellationToken);
        foreach (var person in people)
        {
            var label = PersonLabel(person);
            if (Blank(person.FirstName) || Blank(person.LastName))
                issues.Add(Issue("person.name.missing", CompletenessSeverity.Urgent, RegisterRecordType.Person,
                    person.PersonId, label, "First name or last name is missing.", $"/Persons/Edit?id={person.PersonId}"));

            var contactRoles = new List<string>();
            if (person.PrimaryContactUnits.Count > 0) contactRoles.Add("primary contact");
            if (person.OwnershipContacts.Count > 0) contactRoles.Add("ownership contact");
            if (person.AgentProfile is not null) contactRoles.Add("letting agent");
            if (contactRoles.Count > 0 && Blank(person.PhoneNumber) && Blank(person.Email))
            {
                var relatedUnit = person.PrimaryContactUnits.FirstOrDefault()
                    ?? person.OwnershipContacts.Select(c => c.Ownership.Unit).FirstOrDefault(u => u is not null)
                    ?? person.AgentProfile?.Units.FirstOrDefault();
                issues.Add(relatedUnit is null
                    ? Issue("person.contact-method.missing", CompletenessSeverity.Urgent, RegisterRecordType.Person,
                        person.PersonId, label, $"No phone number or email is recorded for this {string.Join(" and ", contactRoles)}.",
                        $"/Persons/Edit?id={person.PersonId}")
                    : UnitIssue("person.contact-method.missing", CompletenessSeverity.Urgent, RegisterRecordType.Person,
                        person.PersonId, label, $"No phone number or email is recorded for this {string.Join(" and ", contactRoles)}.",
                        $"/Persons/Edit?id={person.PersonId}", relatedUnit));
            }
        }

        var organizations = await context.Organizations
            .Include(o => o.Ownerships).ThenInclude(o => o.Unit)!.ThenInclude(u => u!.Building)!.ThenInclude(b => b!.Property)
            .AsNoTracking().AsSplitQuery().ToListAsync(cancellationToken);
        foreach (var organization in organizations)
        {
            var relatedUnit = organization.Ownerships.Select(o => o.Unit).FirstOrDefault(u => u is not null);
            if (Blank(organization.Name))
                issues.Add(Issue("organization.name.missing", CompletenessSeverity.Urgent, RegisterRecordType.Organization,
                    organization.OrganizationId, OrganizationLabel(organization), "Organization name is missing.", $"/Organizations/Edit?id={organization.OrganizationId}"));
            if (relatedUnit is not null && Blank(organization.RegistrationReference))
                issues.Add(UnitIssue("organization.registration.missing", CompletenessSeverity.Important, RegisterRecordType.Organization,
                    organization.OrganizationId, OrganizationLabel(organization), "Registration reference is missing for a current juristic owner.",
                    $"/Organizations/Edit?id={organization.OrganizationId}", relatedUnit));
            if (relatedUnit is not null && Blank(organization.Address))
                issues.Add(UnitIssue("organization.address.missing", CompletenessSeverity.Important, RegisterRecordType.Organization,
                    organization.OrganizationId, OrganizationLabel(organization), "Address is missing for a current juristic owner.",
                    $"/Organizations/Edit?id={organization.OrganizationId}", relatedUnit));
        }

        var companies = await context.AgentCompanies.Include(c => c.Agents).AsNoTracking().ToListAsync(cancellationToken);
        foreach (var company in companies)
        {
            var label = Blank(company.CompanyName) ? $"Agent company #{company.AgentCompanyId}" : company.CompanyName.Trim();
            if (Blank(company.CompanyName))
                issues.Add(Issue("agent-company.name.missing", CompletenessSeverity.Urgent, RegisterRecordType.AgentCompany,
                    company.AgentCompanyId, label, "Agent company name is missing.", $"/AgentCompanies/Edit?id={company.AgentCompanyId}"));
            if (company.Agents.Count > 0 && Blank(company.Address))
                issues.Add(Issue("agent-company.address.missing", CompletenessSeverity.Important, RegisterRecordType.AgentCompany,
                    company.AgentCompanyId, label, "Address is missing for a company with a current agent profile.", $"/AgentCompanies/Edit?id={company.AgentCompanyId}"));
            if (company.Agents.Count > 0 && Blank(company.RegistrationNumber))
                issues.Add(Issue("agent-company.registration.missing", CompletenessSeverity.Important, RegisterRecordType.AgentCompany,
                    company.AgentCompanyId, label, "Registration number is missing for a company with a current agent profile.", $"/AgentCompanies/Edit?id={company.AgentCompanyId}"));
        }

        var occupancies = await context.Occupancies
            .Include(o => o.Unit)!.ThenInclude(u => u!.Building)!.ThenInclude(b => b!.Property)
            .AsNoTracking().ToListAsync(cancellationToken);
        foreach (var occupancy in occupancies.Where(o => Blank(o.OccupationType) && o.Unit is not null))
            issues.Add(UnitIssue("occupancy.type.missing", CompletenessSeverity.Important, RegisterRecordType.Occupancy,
                occupancy.OccupancyId, $"Occupancy for {UnitLabel(occupancy.Unit!)}", "Occupation type is missing.",
                $"/Occupancies/Edit?id={occupancy.OccupancyId}", occupancy.Unit!));

        var leases = await context.Leases
            .Include(l => l.Unit)!.ThenInclude(u => u!.Building)!.ThenInclude(b => b!.Property)
            .AsNoTracking().ToListAsync(cancellationToken);
        var today = timeProvider.GetLocalNow().Date;
        foreach (var lease in leases.Where(l => l.Unit is not null))
        {
            if (Blank(lease.LeaseHolderName))
                issues.Add(UnitIssue("lease.holder.missing", CompletenessSeverity.Urgent, RegisterRecordType.Lease,
                    lease.LeaseId, $"Lease for {UnitLabel(lease.Unit!)}", "Lease-holder name is missing.", $"/Leases/Edit?id={lease.LeaseId}", lease.Unit!));
            if (lease.StartDate == default)
                issues.Add(UnitIssue("lease.start-date.missing", CompletenessSeverity.Urgent, RegisterRecordType.Lease,
                    lease.LeaseId, $"Lease for {UnitLabel(lease.Unit!)}", "Lease start date is missing.", $"/Leases/Edit?id={lease.LeaseId}", lease.Unit!));
            if (lease.EndDate == default)
                issues.Add(UnitIssue("lease.end-date.missing", CompletenessSeverity.Urgent, RegisterRecordType.Lease,
                    lease.LeaseId, $"Lease for {UnitLabel(lease.Unit!)}", "Lease end date is missing.", $"/Leases/Edit?id={lease.LeaseId}", lease.Unit!));
            else if (lease.EndDate.Date < today)
                issues.Add(UnitIssue("lease.expired", CompletenessSeverity.Urgent, RegisterRecordType.Lease,
                    lease.LeaseId, $"Lease for {UnitLabel(lease.Unit!)}", $"Lease ended on {lease.EndDate:yyyy-MM-dd} and needs review.",
                    $"/Leases/Edit?id={lease.LeaseId}", lease.Unit!));
            if (Blank(lease.EmergencyContactNumber))
                issues.Add(UnitIssue("lease.emergency-contact.missing", CompletenessSeverity.Important, RegisterRecordType.Lease,
                    lease.LeaseId, $"Lease for {UnitLabel(lease.Unit!)}", "Emergency contact number is missing.",
                    $"/Leases/Edit?id={lease.LeaseId}", lease.Unit!));
        }

        var vehicles = await context.Vehicles
            .Include(v => v.Unit)!.ThenInclude(u => u!.Building)!.ThenInclude(b => b!.Property)
            .AsNoTracking().ToListAsync(cancellationToken);
        foreach (var vehicle in vehicles.Where(v => v.Unit is not null))
        {
            if (Blank(vehicle.VehicleRegistration))
                issues.Add(UnitIssue("vehicle.registration.missing", CompletenessSeverity.Urgent, RegisterRecordType.Vehicle,
                    vehicle.VehicleId, VehicleLabel(vehicle), "Vehicle registration is missing.", $"/Vehicles/Edit?id={vehicle.VehicleId}", vehicle.Unit!));
            var missing = new List<string>();
            if (Blank(vehicle.VehicleMake)) missing.Add("make");
            if (Blank(vehicle.VehicleModel)) missing.Add("model");
            if (Blank(vehicle.VehicleColor)) missing.Add("colour");
            if (missing.Count > 0)
                issues.Add(UnitIssue("vehicle.description.incomplete", CompletenessSeverity.Important, RegisterRecordType.Vehicle,
                    vehicle.VehicleId, VehicleLabel(vehicle), $"Vehicle {ListWords(missing)} missing.", $"/Vehicles/Edit?id={vehicle.VehicleId}", vehicle.Unit!));
        }

        var accessCounts = await context.AccessDeviceCounts
            .Include(a => a.Unit)!.ThenInclude(u => u!.Building)!.ThenInclude(b => b!.Property)
            .AsNoTracking().ToListAsync(cancellationToken);
        foreach (var access in accessCounts.Where(a => a.Unit is not null))
        {
            var unknown = new List<string>();
            if (access.OwnershipContactTagCount is null) unknown.Add("ownership-contact tags");
            if (access.OwnershipContactRemoteCount is null) unknown.Add("ownership-contact remotes");
            if (access.OccupantTagCount is null) unknown.Add("occupant tags");
            if (access.OccupantRemoteCount is null) unknown.Add("occupant remotes");
            if (access.AgentTagCount is null) unknown.Add("agent tags");
            if (access.AgentRemoteCount is null) unknown.Add("agent remotes");
            if (unknown.Count > 0)
                issues.Add(UnitIssue("access-count.unknown", CompletenessSeverity.Important, RegisterRecordType.AccessDeviceCount,
                    access.AccessDeviceCountId, $"Access-device counts for {UnitLabel(access.Unit!)}",
                    $"Unknown counts: {string.Join(", ", unknown)}.", $"/AccessDeviceCounts/Edit?id={access.AccessDeviceCountId}", access.Unit!));
        }

        return issues
            .OrderBy(issue => issue.Severity)
            .ThenBy(issue => issue.PropertyName)
            .ThenBy(issue => issue.BuildingName)
            .ThenBy(issue => issue.UnitNumber)
            .ThenBy(issue => issue.RecordType)
            .ThenBy(issue => issue.Code)
            .ToList();

        void AddBuildingIssueIf(bool condition, string code, CompletenessSeverity severity, Building building, string summary)
        {
            if (condition)
                issues.Add(Issue(code, severity, RegisterRecordType.Building, building.BuildingId, BuildingLabel(building), summary,
                    $"/Buildings/Edit?id={building.BuildingId}", building.PropertyId, building.Property?.Name, building.BuildingId, building.Name));
        }

        void AddUnitIssueIf(bool condition, string code, CompletenessSeverity severity, Unit unit, string summary, string actionUrl)
        {
            if (condition)
                issues.Add(UnitIssue(code, severity, RegisterRecordType.Unit, unit.UnitId, UnitLabel(unit), summary, actionUrl, unit));
        }
    }

    private static CompletenessIssue UnitIssue(string code, CompletenessSeverity severity, RegisterRecordType type,
        int id, string label, string summary, string actionUrl, Unit unit) =>
        Issue(code, severity, type, id, label, summary, actionUrl,
            unit.Building?.PropertyId, unit.Building?.Property?.Name,
            unit.BuildingId, unit.Building?.Name, unit.UnitId, unit.UnitNumber);

    private static CompletenessIssue Issue(string code, CompletenessSeverity severity, RegisterRecordType type,
        int id, string label, string summary, string actionUrl,
        int? propertyId = null, string? propertyName = null, int? buildingId = null, string? buildingName = null,
        int? unitId = null, string? unitNumber = null) =>
        new(code, severity, type, id, label, summary, actionUrl,
            propertyId, propertyName, buildingId, buildingName, unitId, unitNumber);

    private static bool Blank(string? value) => string.IsNullOrWhiteSpace(value);
    private static string PropertyLabel(Property property) => Blank(property.Name) ? $"Property #{property.PropertyId}" : property.Name.Trim();
    private static string BuildingLabel(Building building) => Blank(building.Name) ? $"Building #{building.BuildingId}" : building.Name.Trim();
    private static string UnitLabel(Unit unit) => $"{BuildingLabel(unit.Building!)} unit {(Blank(unit.UnitNumber) ? $"#{unit.UnitId}" : unit.UnitNumber.Trim())}";
    private static string PersonLabel(Person person) => Blank($"{person.FirstName} {person.LastName}") ? $"Person #{person.PersonId}" : $"{person.FirstName} {person.LastName}".Trim();
    private static string OrganizationLabel(Organization organization) => Blank(organization.Name) ? $"Organization #{organization.OrganizationId}" : organization.Name.Trim();
    private static string VehicleLabel(Vehicle vehicle) => Blank(vehicle.VehicleRegistration) ? $"Vehicle #{vehicle.VehicleId}" : vehicle.VehicleRegistration.Trim();
    private static string ListWords(IReadOnlyList<string> words) => words.Count == 1
        ? $"{words[0]} is"
        : $"{string.Join(", ", words.Take(words.Count - 1))} and {words[^1]} are";
}
