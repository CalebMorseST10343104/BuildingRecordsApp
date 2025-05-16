using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models;

public class BuildingContext(DbContextOptions<BuildingContext> options) : DbContext(options)
{
    public DbSet<Unit> Units { get; set; }
    public DbSet<Building> Buildings { get; set; }
    public DbSet<Person> Persons { get; set; }
    public DbSet<Ownership> Ownerships { get; set; }
    public DbSet<Owner> Owners { get; set; }
    public DbSet<Agent> Agents { get; set; }
    public DbSet<AgentCompany> AgentCompanies { get; set; }
    public DbSet<CompanyTrust> CompanyTrusts { get; set; }
    public DbSet<Lease> Leases { get; set; }
    public DbSet<Occupancy> Occupancies { get; set; }
    public DbSet<ParkingBay> ParkingBays { get; set; }
    public DbSet<StoreRoom> StoreRooms { get; set; }
    public DbSet<TagRemoteRecord> TagRemoteRecords { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
}
