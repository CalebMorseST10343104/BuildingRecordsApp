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


        #region Unit 1-to-1 Relationships

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

        #endregion

        #region Unit 1-to-Many Relationships

        modelBuilder.Entity<Unit>()
            .HasOne(u => u.Building)
            .WithMany(b => b.Units)
            .HasForeignKey(u => u.BuildingId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Unit>()
            .HasOne(u => u.Agent)
            .WithMany(a => a.Units)
            .HasForeignKey(u => u.AgentId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Unit>()
            .HasOne(u => u.PrimaryContactPerson)
            .WithMany(p => p.PrimaryContactUnits)
            .HasForeignKey(u => u.PrimaryContactPersonId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Unit>()
            .HasMany(u => u.ParkingBays)
            .WithOne(pb => pb.Unit)
            .HasForeignKey(pb => pb.UnitID)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Unit>()
            .HasMany(u => u.StoreRooms)
            .WithOne(sr => sr.Unit)
            .HasForeignKey(sr => sr.UnitId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Unit>()
            .HasMany(u => u.Vehicles)
            .WithOne(v => v.Unit)
            .HasForeignKey(v => v.UnitId)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion

        #region Occupancy

        modelBuilder.Entity<Occupancy>()
        .HasKey(o => new { o.UnitId, o.OccupantId });

        modelBuilder.Entity<Occupancy>()
            .HasOne(o => o.Occupant)
            .WithMany(p => p.Occupancies)
            .HasForeignKey(o => o.OccupantId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Occupancy>()
            .HasOne(o => o.Unit)
            .WithMany(u => u.Occupants)
            .HasForeignKey(o => o.UnitId)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion

        #region Owners

        modelBuilder.Entity<Owner>()
            .HasKey(o => new { o.OwnershipId, o.PersonId });

        modelBuilder.Entity<Owner>()
            .HasOne(o => o.Person)
            .WithMany(p => p.Owners)
            .HasForeignKey(o => o.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Owner>()
            .HasOne(o => o.Ownership)
            .WithMany(o => o.Owners)
            .HasForeignKey(o => o.OwnershipId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Ownership>()
            .HasOne(o => o.CompanyTrust)
            .WithMany(ct => ct.Ownerships)
            .HasForeignKey(o => o.CompanyTrustId)
            .OnDelete(DeleteBehavior.Restrict);

        #endregion

        #region Agent

        modelBuilder.Entity<Agent>()
            .HasOne(a => a.AgentCompany)
            .WithMany(ac => ac.Agents)
            .HasForeignKey(a => a.AgentCompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Agent>()
            .HasMany(a => a.Units)
            .WithOne(u => u.Agent)
            .HasForeignKey(u => u.AgentId)
            .OnDelete(DeleteBehavior.SetNull);

        #endregion
    }
}
