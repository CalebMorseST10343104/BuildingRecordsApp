using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public sealed class BuildingContextFactory : IDesignTimeDbContextFactory<BuildingContext>
{
    public BuildingContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__BuildingContext")
            ?? "Data Source=buildingrecords.db";
        var options = new DbContextOptionsBuilder<BuildingContext>()
            .UseSqlite(connectionString)
            .Options;

        return new BuildingContext(options);
    }
}
