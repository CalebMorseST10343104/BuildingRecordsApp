using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

namespace BuildingRecordsApp.Tests.Services;

public sealed class DatabaseBackupServiceTests : IAsyncLifetime
{
    private readonly string _directory = Path.Combine(Path.GetTempPath(), $"building-backup-tests-{Guid.NewGuid():N}");

    public Task InitializeAsync()
    {
        Directory.CreateDirectory(_directory);
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        Directory.Delete(_directory, recursive: true);
        return Task.CompletedTask;
    }

    [Fact]
    public async Task Backup_is_a_valid_independent_snapshot()
    {
        var databasePath = Path.Combine(_directory, "register.db");
        var options = new DbContextOptionsBuilder<BuildingContext>()
            .UseSqlite($"Data Source={databasePath};Foreign Keys=True")
            .Options;
        await using var context = new BuildingContext(options);
        await context.Database.EnsureCreatedAsync();
        context.Properties.Add(new Property { Name = "Chelsea", Address = "Test" });
        await context.SaveChangesAsync();

        var service = CreateService(context, retainedCount: 5);
        var backup = await service.CreateAsync("manual");
        context.Properties.Add(new Property { Name = "Later change", Address = "Test" });
        await context.SaveChangesAsync();

        var backupPath = service.ResolveExistingBackup(backup.FileName);
        Assert.NotNull(backupPath);
        await using var backupConnection = new SqliteConnection($"Data Source={backupPath}");
        await backupConnection.OpenAsync();
        await using var command = backupConnection.CreateCommand();
        command.CommandText = "SELECT COUNT(*) FROM Properties;";
        Assert.Equal(1L, await command.ExecuteScalarAsync());
    }

    [Fact]
    public async Task Retention_and_path_validation_are_enforced()
    {
        var databasePath = Path.Combine(_directory, "register.db");
        var options = new DbContextOptionsBuilder<BuildingContext>().UseSqlite($"Data Source={databasePath}").Options;
        await using var context = new BuildingContext(options);
        await context.Database.EnsureCreatedAsync();
        var service = CreateService(context, retainedCount: 2);

        await service.CreateAsync("one");
        await service.CreateAsync("two");
        await service.CreateAsync("three");

        Assert.Equal(2, service.List().Count);
        Assert.Null(service.ResolveExistingBackup("../register.db"));
    }

    private DatabaseBackupService CreateService(BuildingContext context, int retainedCount) => new(
        context,
        Options.Create(new DatabaseBackupOptions { Directory = "Backups", RetainedBackupCount = retainedCount }),
        new TestEnvironment(_directory));

    private sealed class TestEnvironment(string contentRoot) : IWebHostEnvironment
    {
        public string ApplicationName { get; set; } = "BuildingRecordsApp.Tests";
        public IFileProvider WebRootFileProvider { get; set; } = new NullFileProvider();
        public string WebRootPath { get; set; } = contentRoot;
        public string EnvironmentName { get; set; } = "Testing";
        public string ContentRootPath { get; set; } = contentRoot;
        public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
    }
}
