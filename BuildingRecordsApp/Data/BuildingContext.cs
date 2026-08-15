using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Models.Entities;

public class BuildingContext(DbContextOptions<BuildingContext> options) : DbContext(options)
{
    public DbSet<Unit> Units { get; set; }
    public DbSet<Building> Buildings { get; set; }
    public DbSet<Person> Persons { get; set; }
    public DbSet<Ownership> Ownerships { get; set; }
    public DbSet<OwnershipContact> Owners { get; set; }
    public DbSet<Agent> Agents { get; set; }
    public DbSet<AgentCompany> AgentCompanies { get; set; }
    public DbSet<Organization> CompanyTrusts { get; set; }
    public DbSet<Property> Properties { get; set; }
    public DbSet<Lease> Leases { get; set; }
    public DbSet<Occupancy> Occupancies { get; set; }
    public DbSet<ParkingBay> ParkingBays { get; set; }
    public DbSet<StoreRoom> StoreRooms { get; set; }
    public DbSet<TagRemoteRecord> TagRemoteRecords { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<Property>().HasIndex(p => p.Name).IsUnique();
        modelBuilder.Entity<Building>().HasIndex(b => new { b.PropertyId, b.Name }).IsUnique();
        modelBuilder.Entity<Unit>().HasIndex(u => new { u.BuildingId, u.UnitNumber }).IsUnique();
        modelBuilder.Entity<ParkingBay>().HasIndex(p => new { p.PropertyId, p.ParkingBayNumber }).IsUnique();
        modelBuilder.Entity<StoreRoom>().HasIndex(s => new { s.PropertyId, s.StoreRoomNumber }).IsUnique();
        modelBuilder.Entity<Vehicle>().HasIndex(v => v.VehicleRegistration).IsUnique();

        modelBuilder.Entity<Building>()
            .HasOne(b => b.Property).WithMany(p => p.Buildings)
            .HasForeignKey(b => b.PropertyId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<ParkingBay>()
            .HasOne(b => b.Property).WithMany(p => p.ParkingBays)
            .HasForeignKey(b => b.PropertyId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<StoreRoom>()
            .HasOne(b => b.Property).WithMany(p => p.StoreRooms)
            .HasForeignKey(b => b.PropertyId).OnDelete(DeleteBehavior.Restrict);

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
            .HasKey(o => o.OccupancyId);

        modelBuilder.Entity<Occupancy>()
            .HasIndex(o => new { o.UnitId, o.OccupantId })
            .IsUnique();

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

        #region Ownership contacts

        modelBuilder.Entity<OwnershipContact>()
            .HasKey(o => o.OwnershipContactId);

        modelBuilder.Entity<OwnershipContact>()
            .HasIndex(o => new { o.OwnershipId, o.PersonId })
            .IsUnique();

        modelBuilder.Entity<OwnershipContact>()
            .HasOne(o => o.Person)
            .WithMany(p => p.OwnershipContacts)
            .HasForeignKey(o => o.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OwnershipContact>()
            .HasOne(o => o.Ownership)
            .WithMany(o => o.OwnershipContacts)
            .HasForeignKey(o => o.OwnershipId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Ownership>()
            .HasOne(o => o.Organization)
            .WithMany(ct => ct.Ownerships)
            .HasForeignKey(o => o.OrganizationId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Ownership>().ToTable(t => t.HasCheckConstraint(
            "CK_Ownership_TypeOrganization",
            "(OwnershipType = 'Natural' AND OrganizationId IS NULL) OR (OwnershipType = 'Juristic' AND OrganizationId IS NOT NULL)"));

        #endregion

        #region Agent

        modelBuilder.Entity<Agent>()
            .HasOne(a => a.Person)
            .WithOne(p => p.AgentProfile)
            .HasForeignKey<Agent>(a => a.PersonId)
            .OnDelete(DeleteBehavior.Restrict);

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
