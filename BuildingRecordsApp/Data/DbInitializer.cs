using BuildingRecordsApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BuildingRecordsApp.Data;

public static class DbInitializer
{
    public static void Initialize(BuildingContext context)
    {
        context.Database.Migrate();

        // Check if there are any records in the database
        if (context.Buildings.Any())
            return; // Database has been seeded
        

        // Seed data in the following order to maintain foreign key constraints
        #region SeedPersons

        var people = new[]
        {
            new Person { FirstName = "John", LastName = "Doe", Email = "foo@example.com", PostalAddress = "123 Main St", IdNumber = "1234567890", PhoneNumber = "1234567890" },
            new Person { FirstName = "Jane", LastName = "Smith", Email = "bar@example.com", PostalAddress = "456 Elm St", IdNumber = "0987654321", PhoneNumber = "0987654321" },
            new Person { FirstName = "Alice", LastName = "Johnson", Email = "fuz@example.com", PostalAddress = "789 Oak St", IdNumber = "1122334455", PhoneNumber = "1122334455" },
            new Person { FirstName = "Bob", LastName = "Brown", Email = "fiz@example.com", PostalAddress = "321 Pine St", IdNumber = "5566778899", PhoneNumber = "5566778899" }, 
            new Person { FirstName = "Charlie", LastName = "Davis", Email = "buz@example.com", PostalAddress = "654 Cedar St", IdNumber = "9988776655", PhoneNumber = "9988776655" }
        };
        context.Persons.AddRange(people);
        context.SaveChanges();

        #endregion
        var property = new Property { Name = "Chelsea", Address = "Shared property" };
        context.Properties.Add(property);
        context.SaveChanges();

        #region SeedBuildings

        var buildings = new[]
        {
            new Building { Name = "Building A", Address = "123 Main St", NumberOfUnits = 10, NumberOfFloors = 3, Property = property },
            new Building { Name = "Building B", Address = "456 Elm St", NumberOfUnits = 20, NumberOfFloors = 5, Property = property },
            new Building { Name = "Building C", Address = "789 Oak St", NumberOfUnits = 15, NumberOfFloors = 4, Property = property },
        };
        context.Buildings.AddRange(buildings);
        context.SaveChanges();

        #endregion
        #region SeedUnits

        var units = new[]
        {
            new Unit { UnitNumber = "101", Bedrooms = 2, DbInverter = true, Housekeeping = false, PetFriendly = true, SublettingAllowed = false, AirconditioningUnits = 1, Building = buildings[0], PrimaryContactPerson = people[0] },
            new Unit { UnitNumber = "102", Bedrooms = 3, DbInverter = false, Housekeeping = true, PetFriendly = false, SublettingAllowed = true, AirconditioningUnits = 2, Building = buildings[0], PrimaryContactPerson = people[1] },
            new Unit { UnitNumber = "201", Bedrooms = 1, DbInverter = true, Housekeeping = false, PetFriendly = true, SublettingAllowed = false, AirconditioningUnits = 1, Building = buildings[1], PrimaryContactPerson = people[2] },
            new Unit { UnitNumber = "202", Bedrooms = 2, DbInverter = false, Housekeeping = true, PetFriendly = false, SublettingAllowed = true, AirconditioningUnits = 2, Building = buildings[1], PrimaryContactPerson = people[3] },
        };
        context.Units.AddRange(units);
        context.SaveChanges();

        #endregion
        #region SeedOwnerships

        var organization = new Organization { Name = "Company A", Address = "456 Elm St", RegistrationReference = "987654321" };
        context.CompanyTrusts.Add(organization);
        context.SaveChanges();

        var ownerships = new[]
        {
            new Ownership { Unit = units[0], OwnershipType = "Natural" },
            new Ownership { Unit = units[1], OwnershipType = "Natural" },
            new Ownership { Unit = units[2], OwnershipType = "Juristic", Organization = organization },
            new Ownership { Unit = units[3], OwnershipType = "Juristic", Organization = organization }
        };
        context.Ownerships.AddRange(ownerships);
        context.SaveChanges();

        #endregion
        #region SeedOwners

        var owners = new[]
        {
            new OwnershipContact { Person = people[0], Ownership = ownerships[0] },
            new OwnershipContact { Person = people[1], Ownership = ownerships[1] },
            new OwnershipContact { Person = people[2], Ownership = ownerships[2] },
            new OwnershipContact { Person = people[3], Ownership = ownerships[3] },
            new OwnershipContact { Person = people[4], Ownership = ownerships[0] }
        };
        context.Owners.AddRange(owners);
        context.SaveChanges();

        #endregion
        #region SeedAgentCompanies

        var agentCompanies = new[]
        {
            new AgentCompany { CompanyName = "Agent Company A", Address = "123 Main St", RegistrationNumber = "123456789" },
            new AgentCompany { CompanyName = "Agent Company B", Address = "456 Elm St", RegistrationNumber = "987654321" }
        };
        context.AgentCompanies.AddRange(agentCompanies);
        context.SaveChanges();

        #endregion
        #region SeedAgents
        
        var agents = new[]
        {
            new Agent { Person = people[2], AgentCompany = agentCompanies[0] },
            new Agent { Person = people[3], AgentCompany = agentCompanies[1] },
            new Agent { Person = people[4], AgentCompany = agentCompanies[0] }
        };
        context.Agents.AddRange(agents);
        context.SaveChanges();

        #endregion
        #region SeedCompanyTrusts
        
        // Organizations are seeded before juristic ownerships.

        #endregion
        #region SeedLeases
        
        var leases = new[]
        {
            new Lease { LeaseHolderName = "John Doe", StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(1), DeclaredOccupantCount = 2, SignedRules = true, PetsPresent = false, EmergencyContactNumber = "1234567890", Unit = units[0] },
            new Lease { LeaseHolderName = "Jane Smith", StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(1), DeclaredOccupantCount = 3, SignedRules = false, PetsPresent = true, EmergencyContactNumber = "0987654321", Unit = units[1] },
            new Lease { LeaseHolderName = "Alice Johnson", StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(1), DeclaredOccupantCount = 1, SignedRules = true, PetsPresent = false, EmergencyContactNumber = "1122334455", Unit = units[2] },
            new Lease { LeaseHolderName = "Bob Brown", StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(1), DeclaredOccupantCount = 2, SignedRules = true, PetsPresent = true, EmergencyContactNumber = "5566778899", Unit = units[3] }
        };
        context.Leases.AddRange(leases);
        context.SaveChanges();

        #endregion
        #region SeedOccupancies

        var occupancies = new[]
        {
            new Occupancy { OccupationType = "OwnershipContact", Unit = units[0], Occupant = people[0] },
            new Occupancy { OccupationType = "Tenant", Unit = units[1], Occupant = people[1] },
            new Occupancy { OccupationType = "Tenant", Unit = units[2], Occupant = people[2] },
            new Occupancy { OccupationType = "OwnershipContact", Unit = units[3], Occupant = people[3] }
        };
        context.Occupancies.AddRange(occupancies);
        context.SaveChanges();

        #endregion
        #region SeedParkingBays
        
        var parkingBays = new[]
        {
            new ParkingBay { ParkingBayNumber = "P1", Unit = units[0], IsNearEntrance = true, Property = property },
            new ParkingBay { ParkingBayNumber = "P2", Unit = units[1], Property = property },
            new ParkingBay { ParkingBayNumber = "P3", Unit = units[2], Property = property },
            new ParkingBay { ParkingBayNumber = "P4", Unit = units[3], Property = property }
        };
        context.ParkingBays.AddRange(parkingBays);
        context.SaveChanges();

        #endregion
        #region SeedStoreRooms
        
        var storeRooms = new[]
        {
            new StoreRoom { StoreRoomNumber = "SR1", Unit = units[0], Property = property },
            new StoreRoom { StoreRoomNumber = "SR2", Unit = units[1], Property = property },
            new StoreRoom { StoreRoomNumber = "SR3", Unit = units[2], Property = property },
            new StoreRoom { StoreRoomNumber = "SR4", Unit = units[3], Property = property }
        };
        context.StoreRooms.AddRange(storeRooms);
        context.SaveChanges();

        #endregion
        #region SeedTagRemoteRecords
        
        var tagRemoteRecords = new[]
        {
            new TagRemoteRecord { TagsOwner = 1, RemotesOwner = 2, TagsOccupant = 3, RemotesOccupant = 4, TagsAgent = 5, RemotesAgent = 6, Unit = units[0] },
            new TagRemoteRecord { TagsOwner = 7, RemotesOwner = 8, TagsOccupant = 9, RemotesOccupant = 10, TagsAgent = 11, RemotesAgent = 12, Unit = units[1] },
            new TagRemoteRecord { Unit = units[2] },
            new TagRemoteRecord { Unit = units[3] }
        };
        context.TagRemoteRecords.AddRange(tagRemoteRecords);
        context.SaveChanges();

        #endregion
        #region SeedVehicles
        
        var vehicles = new[]
        {
            new Vehicle { VehicleRegistration = "ABC123", VehicleModel = "Toyota", VehicleMake = "Corolla", VehicleColor = "Red", Unit = units[0] },
            new Vehicle { VehicleRegistration = "XYZ789", VehicleModel = "Honda", VehicleMake = "Civic", VehicleColor = "Blue", Unit = units[1] },
            new Vehicle { VehicleRegistration = "LMN456", VehicleModel = "Ford", VehicleMake = "Focus", VehicleColor = "Green", Unit = units[2] },
            new Vehicle { VehicleRegistration = "DEF321", VehicleModel = "Chevrolet", VehicleMake = "Malibu", VehicleColor = "Black", Unit = units[3] }
        };
        context.Vehicles.AddRange(vehicles);
        context.SaveChanges();

        #endregion
        context.SaveChanges();
    }
}
