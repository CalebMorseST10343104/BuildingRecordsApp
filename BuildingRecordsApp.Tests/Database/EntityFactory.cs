using BuildingRecordsApp.Models.Entities;

namespace BuildingRecordsApp.Tests.Database;

internal static class EntityFactory
{
    public static Property Property(string name = "Chelsea") => new() { Name = name, Address = "Test address" };
    public static Building Building(Property property, string name = "A") => new() { Property = property, Name = name };
    public static Unit Unit(Building building, string number) => new() { Building = building, UnitNumber = number };
    public static Person Person(string name) => new() { FirstName = name, LastName = "Test" };
    public static AgentCompany AgentCompany() => new() { CompanyName = "Test Letting", RegistrationNumber = "TEST-1" };
}
