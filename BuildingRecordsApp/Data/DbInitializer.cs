using System;
using BuildingRecordsApp.Models;
using Microsoft.VisualBasic;

namespace BuildingRecordsApp.Data;

public static class DbInitializer
{
    public static void Initialize(BuildingContext context)
    {
        // Check if the database exists
        context.Database.EnsureCreated();

        // Check if there are any records in the database
        if (context.Buildings.Any())
            return; // Database has been seeded
        

        // Seed data in the following order to maintain foreign key constraints
        #region SeedPersons

        var people = new[]
        {
            new Person { Name = "John", Surname = "Doe", Email = "foo@example.com", PostalAddress = "123 Main St", IdNumber = "1234567890", PhoneNumber = "1234567890" },
            new Person { Name = "Jane", Surname = "Smith", Email = "bar@example.com", PostalAddress = "456 Elm St", IdNumber = "0987654321", PhoneNumber = "0987654321" },
            new Person { Name = "Alice", Surname = "Johnson", Email = "fuz@example.com", PostalAddress = "789 Oak St", IdNumber = "1122334455", PhoneNumber = "1122334455" },
            new Person { Name = "Bob", Surname = "Brown", Email = "fiz@example.com", PostalAddress = "321 Pine St", IdNumber = "5566778899", PhoneNumber = "5566778899" }, 
            new Person { Name = "Charlie", Surname = "Davis", Email = "buz@example.com", PostalAddress = "654 Cedar St", IdNumber = "9988776655", PhoneNumber = "9988776655" }
        };
        context.Persons.AddRange(people);
        context.SaveChanges();

        #endregion
        #region SeedBuildings

        var buildings = new[]
        {
            new Building { Name = "Building A", Address = "123 Main St", NumberOfUnits = 10, NumberOfFloors = 3 },
            new Building { Name = "Building B", Address = "456 Elm St", NumberOfUnits = 20, NumberOfFloors = 5 },
            new Building { Name = "Building C", Address = "789 Oak St", NumberOfUnits = 15, NumberOfFloors = 4 },
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

        var ownerships = new[]
        {
            new Ownership { Unit = units[0], OwnershipType = "Natural" },
            new Ownership { Unit = units[1], OwnershipType = "Natural" },
            new Ownership { Unit = units[2], OwnershipType = "Juristic" },
            new Ownership { Unit = units[3], OwnershipType = "Juristic" }
        };
        context.Ownerships.AddRange(ownerships);
        context.SaveChanges();

        #endregion
        #region SeedOwners

        var owners = new[]
        {
            new Owner { Person = people[0], Ownership = ownerships[0] },
            new Owner { Person = people[1], Ownership = ownerships[1] },
            new Owner { Person = people[2], Ownership = ownerships[2] },
            new Owner { Person = people[3], Ownership = ownerships[3] },
            new Owner { Person = people[4], Ownership = ownerships[0] }
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
            new Agent { FirstName = "Alice", LastName = "Smith", AgentCompany = agentCompanies[0], Email = "foo@example.net", PhoneNumber = "1234567890" },
            new Agent { FirstName = "Bob", LastName = "Johnson", AgentCompany = agentCompanies[1], Email = "bar@example.net", PhoneNumber = "0987654321" },
            new Agent { FirstName = "Charlie", LastName = "Brown", AgentCompany = agentCompanies[0], Email = "fiz@example.net", PhoneNumber = "1122334455" }
        };
        context.Agents.AddRange(agents);
        context.SaveChanges();

        #endregion
        #region SeedCompanyTrusts
        
        var companyTrusts = new[]
        {
            new CompanyTrust { Name = "Company A", Address = "456 Elm St", RegistrationNumber = "987654321", Ownerships = [ownerships[2], ownerships[3]]}
        };
        context.CompanyTrusts.AddRange(companyTrusts);
        context.SaveChanges();

        #endregion
        #region SeedLeases
        
        var leases = new[]
        {
            new Lease { LeaseHolderName = "John Doe", StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(1), PersonsOccupying = 2, SignedRules = true, AllowedPets = false, EmergencyContactNumber = "1234567890", Unit = units[0] },
            new Lease { LeaseHolderName = "Jane Smith", StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(1), PersonsOccupying = 3, SignedRules = false, AllowedPets = true, EmergencyContactNumber = "0987654321", Unit = units[1] },
            new Lease { LeaseHolderName = "Alice Johnson", StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(1), PersonsOccupying = 1, SignedRules = true, AllowedPets = false, EmergencyContactNumber = "1122334455", Unit = units[2] },
            new Lease { LeaseHolderName = "Bob Brown", StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(1), PersonsOccupying = 2, SignedRules = true, AllowedPets = true, EmergencyContactNumber = "5566778899", Unit = units[3] }
        };
        context.Leases.AddRange(leases);
        context.SaveChanges();

        #endregion
        #region SeedOccupancies

        var occupancies = new[]
        {
            new Occupancy { OccupationType = "Owner", Unit = units[0], Occupant = people[0] },
            new Occupancy { OccupationType = "Tenant", Unit = units[1], Occupant = people[1] },
            new Occupancy { OccupationType = "Tenant", Unit = units[2], Occupant = people[2] },
            new Occupancy { OccupationType = "Owner", Unit = units[3], Occupant = people[3] }
        };
        context.Occupancies.AddRange(occupancies);
        context.SaveChanges();

        #endregion
        #region SeedParkingBays
        
        var parkingBays = new[]
        {
            new ParkingBay { ParkingBayNumber = "P1", Unit = units[0] },
            new ParkingBay { ParkingBayNumber = "P2", Unit = units[1] },
            new ParkingBay { ParkingBayNumber = "P3", Unit = units[2] },
            new ParkingBay { ParkingBayNumber = "P4", Unit = units[3] }
        };
        context.ParkingBays.AddRange(parkingBays);
        context.SaveChanges();

        #endregion
        #region SeedStoreRooms
        
        var storeRooms = new[]
        {
            new StoreRoom { StoreRoomNumber = "SR1", Unit = units[0] },
            new StoreRoom { StoreRoomNumber = "SR2", Unit = units[1] },
            new StoreRoom { StoreRoomNumber = "SR3", Unit = units[2] },
            new StoreRoom { StoreRoomNumber = "SR4", Unit = units[3] }
        };
        context.StoreRooms.AddRange(storeRooms);
        context.SaveChanges();

        #endregion
        #region SeedTagRemoteRecords
        
        var tagRemoteRecords = new[]
        {
            new TagRemoteRecord { TagsOwner = 1, RemotesOwner = 2, TagsOccupant = 3, RemotesOccupant = 4, TagsAgent = 5, RemotesAgent = 6, Unit = units[0] },
            new TagRemoteRecord { TagsOwner = 7, RemotesOwner = 8, TagsOccupant = 9, RemotesOccupant = 10, TagsAgent = 11, RemotesAgent = 12, Unit = units[1] }
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
