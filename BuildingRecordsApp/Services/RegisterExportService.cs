using BuildingRecordsApp.Data;
using BuildingRecordsApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BuildingRecordsApp.Services;

public sealed class RegisterExportService(BuildingContext context) : IRegisterExportService
{
    public async Task<RegisterExportResult> ExportExcelAsync(
        int propertyId,
        IReadOnlyCollection<int> buildingIds,
        CancellationToken cancellationToken = default)
    {
        var property = await context.Properties.AsNoTracking()
            .Include(item => item.Buildings)
            .SingleOrDefaultAsync(item => item.PropertyId == propertyId, cancellationToken)
            ?? throw new ArgumentException("The selected property does not exist.", nameof(propertyId));

        var validBuildingIds = property.Buildings.Select(item => item.BuildingId).ToHashSet();
        var selectedBuildingIds = buildingIds.Count == 0
            ? validBuildingIds
            : buildingIds.Where(validBuildingIds.Contains).ToHashSet();
        if (buildingIds.Count > 0 && selectedBuildingIds.Count != buildingIds.Distinct().Count())
            throw new ArgumentException("One or more selected buildings do not belong to the selected property.", nameof(buildingIds));

        var units = await context.Units.AsNoTracking().AsSplitQuery()
            .Where(unit => selectedBuildingIds.Contains(unit.BuildingId))
            .Include(unit => unit.Building).ThenInclude(building => building!.Property)
            .Include(unit => unit.PrimaryContactPerson)
            .Include(unit => unit.Ownership).ThenInclude(ownership => ownership!.Organization)
            .Include(unit => unit.Ownership).ThenInclude(ownership => ownership!.OwnershipContacts).ThenInclude(contact => contact.Person)
            .Include(unit => unit.Agent).ThenInclude(agent => agent!.Person)
            .Include(unit => unit.Agent).ThenInclude(agent => agent!.AgentCompany)
            .Include(unit => unit.Lease)
            .Include(unit => unit.AccessDeviceCount)
            .Include(unit => unit.Occupants).ThenInclude(occupancy => occupancy.Occupant)
            .Include(unit => unit.ParkingBays)
            .Include(unit => unit.StoreRooms)
            .Include(unit => unit.Vehicles)
            .OrderBy(unit => unit.Building!.Name).ThenBy(unit => unit.UnitNumber)
            .ToListAsync(cancellationToken);

        var unitIds = units.Select(unit => unit.UnitId).ToHashSet();
        var includeAllPropertyInfrastructure = buildingIds.Count == 0 || selectedBuildingIds.SetEquals(validBuildingIds);
        var parkingBays = await context.ParkingBays.AsNoTracking()
            .Include(item => item.Unit).ThenInclude(unit => unit!.Building)
            .Where(item => item.PropertyId == propertyId &&
                (includeAllPropertyInfrastructure || (item.UnitID != null && unitIds.Contains(item.UnitID.Value))))
            .OrderBy(item => item.ParkingBayNumber).ToListAsync(cancellationToken);
        var storeRooms = await context.StoreRooms.AsNoTracking()
            .Include(item => item.Unit).ThenInclude(unit => unit!.Building)
            .Where(item => item.PropertyId == propertyId &&
                (includeAllPropertyInfrastructure || (item.UnitId != null && unitIds.Contains(item.UnitId.Value))))
            .OrderBy(item => item.StoreRoomNumber).ToListAsync(cancellationToken);

        var worksheets = new List<ExportWorksheet>
        {
            MainSheet(units),
            PeopleSheet(units),
            NaturalOwnershipSheet(units),
            JuristicOwnershipSheet(units),
            AgentsSheet(units),
            OccupantsSheet(units),
            LeasesSheet(units),
            VehiclesSheet(units),
            AccessDevicesSheet(units),
            BasementSheet(parkingBays, storeRooms)
        };

        var scopeName = selectedBuildingIds.SetEquals(validBuildingIds)
            ? property.Name
            : $"{property.Name}-{string.Join("-", property.Buildings.Where(item => selectedBuildingIds.Contains(item.BuildingId)).Select(item => item.Name))}";
        var safeScope = string.Concat(scopeName.Select(character => char.IsLetterOrDigit(character) ? character : '-')).Trim('-');
        var fileName = $"Building-register-{(string.IsNullOrWhiteSpace(safeScope) ? "export" : safeScope)}-{DateTime.Now:yyyy-MM-dd}.xlsx";
        return new RegisterExportResult(RegisterExcelWorkbookWriter.Write(worksheets), fileName);
    }

    private static ExportWorksheet MainSheet(IReadOnlyList<Unit> units)
    {
        var rows = Rows(
            ["Property", "Building", "Unit", "Bedrooms", "Parking Bays", "Storerooms", "Primary Contact", "Phone", "Natural Ownership Contacts", "Organization", "Juristic Ownership Contacts", "Agent Company", "Agent", "Agent Phone", "Occupation Types", "Occupant Count", "Occupants", "Lease Start", "Lease End", "Lease Holder", "Declared Occupants", "Rules Signed", "Pets Present", "Emergency Contact", "Vehicles", "Ownership Tags", "Ownership Remotes", "Occupant Tags", "Occupant Remotes", "Agent Tags", "Agent Remotes", "DB Inverter", "Housekeeping", "Pet Friendly", "Subletting Allowed", "AC Units"]);
        foreach (var unit in units)
        {
            var ownership = unit.Ownership;
            var ownershipContacts = ownership?.OwnershipContacts.Select(item => FullName(item.Person)) ?? [];
            rows.Add([
                unit.Building?.Property.Name, unit.Building?.Name, unit.UnitNumber, unit.Bedrooms,
                Join(unit.ParkingBays.Select(item => item.ParkingBayNumber)), Join(unit.StoreRooms.Select(item => item.StoreRoomNumber)),
                FullName(unit.PrimaryContactPerson), unit.PrimaryContactPerson?.PhoneNumber,
                IsNatural(ownership) ? Join(ownershipContacts) : null,
                IsJuristic(ownership) ? ownership?.Organization?.Name : null,
                IsJuristic(ownership) ? Join(ownershipContacts) : null,
                unit.Agent?.AgentCompany.CompanyName, FullName(unit.Agent?.Person), unit.Agent?.Person.PhoneNumber,
                Join(unit.Occupants.Select(item => item.OccupationType)), unit.Occupants.Count,
                Join(unit.Occupants.Select(item => FullName(item.Occupant))),
                unit.Lease?.StartDate, unit.Lease?.EndDate, unit.Lease?.LeaseHolderName,
                unit.Lease?.DeclaredOccupantCount, unit.Lease?.SignedRules, unit.Lease?.PetsPresent, unit.Lease?.EmergencyContactNumber,
                Join(unit.Vehicles.Select(VehicleDescription)),
                unit.AccessDeviceCount?.OwnershipContactTagCount, unit.AccessDeviceCount?.OwnershipContactRemoteCount,
                unit.AccessDeviceCount?.OccupantTagCount, unit.AccessDeviceCount?.OccupantRemoteCount,
                unit.AccessDeviceCount?.AgentTagCount, unit.AccessDeviceCount?.AgentRemoteCount,
                unit.DbInverter, unit.Housekeeping, unit.PetFriendly, unit.SublettingAllowed, unit.AirconditioningUnits
            ]);
        }
        return new ExportWorksheet("MAIN", rows, 1,
        [
            (1, 8, "UNIT INFORMATION", 11), (9, 9, "NATURAL OWNERSHIP", 4),
            (10, 11, "JURISTIC OWNERSHIP", 5), (12, 14, "AGENT INFORMATION", 6),
            (15, 17, "OCCUPANT DETAILS", 7), (18, 24, "LEASE INFORMATION", 8),
            (25, 25, "VEHICLE INFORMATION", 6), (26, 31, "TAG AND REMOTE COUNTS", 12),
            (32, 36, "OTHER UNIT INFORMATION", 9)
        ]);
    }

    private static ExportWorksheet PeopleSheet(IReadOnlyList<Unit> units)
    {
        var people = units.SelectMany(unit =>
                unit.Occupants.Select(item => item.Occupant)
                    .Concat(unit.Ownership?.OwnershipContacts.Select(item => item.Person) ?? [])
                    .Concat(unit.PrimaryContactPerson is null ? [] : [unit.PrimaryContactPerson])
                    .Concat(unit.Agent?.Person is null ? [] : [unit.Agent.Person]))
            .Where(person => person is not null).Cast<Person>().DistinctBy(person => person.PersonId)
            .OrderBy(person => person.LastName).ThenBy(person => person.FirstName);
        var rows = Rows(["Person ID", "First Name", "Last Name", "Full Name", "ID / Passport", "Email Address", "Mobile Number", "Postal Address"]);
        foreach (var person in people)
            rows.Add([person.PersonId, person.FirstName, person.LastName, FullName(person), person.IdNumber, person.Email, person.PhoneNumber, person.PostalAddress]);
        return new ExportWorksheet("PEOPLE", rows);
    }

    private static ExportWorksheet NaturalOwnershipSheet(IReadOnlyList<Unit> units)
    {
        var rows = Rows(["Property", "Building", "Unit", "Person ID", "Full Name", "ID / Passport", "Email Address", "Mobile Number"]);
        foreach (var unit in units.Where(unit => IsNatural(unit.Ownership)))
        foreach (var contact in unit.Ownership!.OwnershipContacts.OrderBy(item => item.Person.LastName))
            rows.Add([unit.Building?.Property.Name, unit.Building?.Name, unit.UnitNumber, contact.PersonId, FullName(contact.Person), contact.Person.IdNumber, contact.Person.Email, contact.Person.PhoneNumber]);
        return new ExportWorksheet("NATURAL OWNERSHIP", rows);
    }

    private static ExportWorksheet JuristicOwnershipSheet(IReadOnlyList<Unit> units)
    {
        var rows = Rows(["Property", "Building", "Unit", "Organization", "Organization Type", "Registration Reference", "Country", "Organization Address", "Contact Person ID", "Contact Name", "ID / Passport", "Email Address", "Mobile Number"]);
        foreach (var unit in units.Where(unit => IsJuristic(unit.Ownership)))
        {
            var contacts = unit.Ownership!.OwnershipContacts.OrderBy(item => item.Person.LastName).ToList();
            if (contacts.Count == 0)
                rows.Add(JuristicRow(unit, null));
            else
                foreach (var contact in contacts) rows.Add(JuristicRow(unit, contact.Person));
        }
        return new ExportWorksheet("JURISTIC OWNERSHIP", rows);
    }

    private static IReadOnlyList<object?> JuristicRow(Unit unit, Person? person) =>
    [
        unit.Building?.Property.Name, unit.Building?.Name, unit.UnitNumber, unit.Ownership?.Organization?.Name,
        unit.Ownership?.Organization?.OrganizationType, unit.Ownership?.Organization?.RegistrationReference,
        unit.Ownership?.Organization?.Country, unit.Ownership?.Organization?.Address,
        person?.PersonId, FullName(person), person?.IdNumber, person?.Email, person?.PhoneNumber
    ];

    private static ExportWorksheet AgentsSheet(IReadOnlyList<Unit> units)
    {
        var rows = Rows(["Property", "Building", "Unit", "Agent ID", "Agent Company", "Company Registration", "Agent Name", "Email Address", "Mobile Number"]);
        foreach (var unit in units.Where(unit => unit.Agent is not null))
            rows.Add([unit.Building?.Property.Name, unit.Building?.Name, unit.UnitNumber, unit.Agent!.AgentId, unit.Agent.AgentCompany.CompanyName, unit.Agent.AgentCompany.RegistrationNumber, FullName(unit.Agent.Person), unit.Agent.Person.Email, unit.Agent.Person.PhoneNumber]);
        return new ExportWorksheet("AGENTS", rows);
    }

    private static ExportWorksheet OccupantsSheet(IReadOnlyList<Unit> units)
    {
        var rows = Rows(["Property", "Building", "Unit", "Person ID", "Full Name", "Occupation Type", "ID / Passport", "Email Address", "Mobile Number"]);
        foreach (var unit in units)
        foreach (var occupancy in unit.Occupants.OrderBy(item => item.Occupant!.LastName))
            rows.Add([unit.Building?.Property.Name, unit.Building?.Name, unit.UnitNumber, occupancy.OccupantId, FullName(occupancy.Occupant), occupancy.OccupationType, occupancy.Occupant?.IdNumber, occupancy.Occupant?.Email, occupancy.Occupant?.PhoneNumber]);
        return new ExportWorksheet("OCCUPANTS", rows);
    }

    private static ExportWorksheet LeasesSheet(IReadOnlyList<Unit> units)
    {
        var rows = Rows(["Property", "Building", "Unit", "Lease Start", "Lease End", "Lease Holder", "Declared Occupants", "Conduct Rules Signed", "Pets Present", "Emergency Contact Number"]);
        foreach (var unit in units.Where(unit => unit.Lease is not null))
            rows.Add([unit.Building?.Property.Name, unit.Building?.Name, unit.UnitNumber, unit.Lease!.StartDate, unit.Lease.EndDate, unit.Lease.LeaseHolderName, unit.Lease.DeclaredOccupantCount, unit.Lease.SignedRules, unit.Lease.PetsPresent, unit.Lease.EmergencyContactNumber]);
        return new ExportWorksheet("LEASES", rows);
    }

    private static ExportWorksheet VehiclesSheet(IReadOnlyList<Unit> units)
    {
        var rows = Rows(["Property", "Building", "Unit", "Registration", "Make", "Model", "Colour", "Description"]);
        foreach (var unit in units)
        foreach (var vehicle in unit.Vehicles.OrderBy(item => item.VehicleRegistration))
            rows.Add([unit.Building?.Property.Name, unit.Building?.Name, unit.UnitNumber, vehicle.VehicleRegistration, vehicle.VehicleMake, vehicle.VehicleModel, vehicle.VehicleColor, VehicleDescription(vehicle)]);
        return new ExportWorksheet("VEHICLES", rows);
    }

    private static ExportWorksheet AccessDevicesSheet(IReadOnlyList<Unit> units)
    {
        var rows = Rows(["Property", "Building", "Unit", "Ownership Contact Tags", "Ownership Contact Remotes", "Occupant Tags", "Occupant Remotes", "Agent Tags", "Agent Remotes"]);
        foreach (var unit in units)
            rows.Add([unit.Building?.Property.Name, unit.Building?.Name, unit.UnitNumber, unit.AccessDeviceCount?.OwnershipContactTagCount, unit.AccessDeviceCount?.OwnershipContactRemoteCount, unit.AccessDeviceCount?.OccupantTagCount, unit.AccessDeviceCount?.OccupantRemoteCount, unit.AccessDeviceCount?.AgentTagCount, unit.AccessDeviceCount?.AgentRemoteCount]);
        return new ExportWorksheet("TAG AND REMOTE", rows);
    }

    private static ExportWorksheet BasementSheet(IReadOnlyList<ParkingBay> bays, IReadOnlyList<StoreRoom> rooms)
    {
        var rows = Rows(["Type", "Number", "Allocated Building", "Allocated Unit", "Near Entrance"]);
        rows.AddRange(bays.Select(item => (IReadOnlyList<object?>)["Parking Bay", item.ParkingBayNumber, item.Unit?.Building?.Name, item.Unit?.UnitNumber, item.IsNearEntrance]));
        rows.AddRange(rooms.Select(item => (IReadOnlyList<object?>)["Storeroom", item.StoreRoomNumber, item.Unit?.Building?.Name, item.Unit?.UnitNumber, null]));
        return new ExportWorksheet("BASEMENT", rows);
    }

    private static List<IReadOnlyList<object?>> Rows(IReadOnlyList<object?> headers) => [headers];
    private static bool IsNatural(Ownership? ownership) => string.Equals(ownership?.OwnershipType, "Natural", StringComparison.OrdinalIgnoreCase);
    private static bool IsJuristic(Ownership? ownership) => string.Equals(ownership?.OwnershipType, "Juristic", StringComparison.OrdinalIgnoreCase);
    private static string? FullName(Person? person) => person is null ? null : string.Join(" ", new[] { person.FirstName, person.LastName }.Where(value => !string.IsNullOrWhiteSpace(value)));
    private static string Join(IEnumerable<string?> values) => string.Join(", ", values.Where(value => !string.IsNullOrWhiteSpace(value)).Distinct());
    private static string VehicleDescription(Vehicle vehicle) =>
        string.Join(" ", new[] { vehicle.VehicleColor, vehicle.VehicleMake, vehicle.VehicleModel }
            .Where(value => !string.IsNullOrWhiteSpace(value)));
}
