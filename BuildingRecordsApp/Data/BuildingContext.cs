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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Unit>()
            .HasOne(u => u.TagRemoteRecord)
            .WithOne(tr => tr.Unit)
            .HasForeignKey<TagRemoteRecord>(tr => tr.UnitId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Unit>()
            .HasOne(u => u.Lease)
            .WithOne(l => l.Unit)
            .HasForeignKey<Lease>(l => l.UnitId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Unit>()
            .HasOne(u => u.Ownership)
            .WithOne(o => o.Unit)
            .HasForeignKey<Ownership>(o => o.UnitId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Unit>()
            .HasOne(u => u.Agent)
            .WithMany(a => a.Units)
            .HasForeignKey(u => u.AgentId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Unit>()
            .HasOne(u => u.Building)
            .WithMany(b => b.Units)
            .HasForeignKey(u => u.BuildingId)
            .OnDelete(DeleteBehavior.Restrict);
            
        modelBuilder.Entity<Occupancy>()
            .HasOne(o => o.Occupant)
            .WithMany() 
            .HasForeignKey(o => o.OccupantId)
            .OnDelete(DeleteBehavior.Restrict); 

        modelBuilder.Entity<Occupancy>()
            .HasOne(o => o.Unit)
            .WithMany() 
            .HasForeignKey(o => o.UnitId)
            .OnDelete(DeleteBehavior.Restrict); 
    }
}
