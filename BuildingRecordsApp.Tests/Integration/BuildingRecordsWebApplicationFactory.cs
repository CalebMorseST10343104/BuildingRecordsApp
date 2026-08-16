using BuildingRecordsApp.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace BuildingRecordsApp.Tests.Integration;

internal sealed class BuildingRecordsWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _workingDirectory = Path.Combine(Path.GetTempPath(), $"building-records-tests-{Guid.NewGuid():N}");

    public string DatabasePath => Path.Combine(_workingDirectory, "integration.db");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Directory.CreateDirectory(_workingDirectory);
        builder.UseEnvironment("Testing");
        builder.ConfigureAppConfiguration((_, configuration) =>
        {
            configuration.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:BuildingContext"] = $"Data Source={DatabasePath};Foreign Keys=True",
                ["Database:SeedSampleData"] = "false",
                ["DatabaseBackups:Directory"] = Path.Combine(_workingDirectory, "backups"),
                ["DesktopDeployment:UseHttpsRedirection"] = "false"
            });
        });
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<BuildingContext>();
            services.RemoveAll<DbContextOptions<BuildingContext>>();
            services.AddDbContext<BuildingContext>(options => options.UseSqlite(
                $"Data Source={DatabasePath};Foreign Keys=True"));
        });
    }

    public async Task WithDatabaseAsync(Func<BuildingContext, Task> action)
    {
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BuildingContext>();
        await action(context);
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);
        using var scope = host.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BuildingContext>();
        var migrationProperty = context.Properties.SingleOrDefault(p => p.Name == "Chelsea");
        if (migrationProperty is not null && string.IsNullOrWhiteSpace(migrationProperty.Address))
        {
            migrationProperty.Address = "Integration-test baseline";
            context.SaveChanges();
        }

        return host;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing && Directory.Exists(_workingDirectory))
            Directory.Delete(_workingDirectory, recursive: true);
    }
}
