using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Services;
using BuildingRecordsApp.Data;
using Microsoft.Data.Sqlite;


public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        // Add services to the container.
        builder.Services.AddRazorPages(options =>
        {
            foreach (var action in new[] { "Index", "Create", "Edit", "Delete" })
            {
                options.Conventions.AddPageRoute($"/Organizations/{action}", $"/CompanyTrusts/{action}");
                options.Conventions.AddPageRoute($"/OwnershipContacts/{action}", $"/Owners/{action}");
                options.Conventions.AddPageRoute($"/AccessDeviceCounts/{action}", $"/TagRemoteRecords/{action}");
            }

            options.Conventions.AddPageRoute("/Organizations/Index", "/CompanyTrusts");
            options.Conventions.AddPageRoute("/OwnershipContacts/Index", "/Owners");
            options.Conventions.AddPageRoute("/AccessDeviceCounts/Index", "/TagRemoteRecords");
        });
        
        var configuredConnection = builder.Configuration.GetConnectionString("BuildingContext")
            ?? throw new InvalidOperationException("ConnectionStrings:BuildingContext is required.");
        var connectionSettings = new SqliteConnectionStringBuilder(configuredConnection);
        if (!Path.IsPathRooted(connectionSettings.DataSource) && connectionSettings.DataSource != ":memory:")
            connectionSettings.DataSource = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, connectionSettings.DataSource));
        var resolvedConnection = connectionSettings.ToString();

        builder.Services.AddDbContext<BuildingContext>(options =>
        {
            options.UseSqlite(resolvedConnection);
        });
        builder.Services.Configure<DatabaseBackupOptions>(builder.Configuration.GetSection("DatabaseBackups"));
        builder.Services.Configure<DesktopDeploymentOptions>(builder.Configuration.GetSection("DesktopDeployment"));
        builder.Services.AddScoped<IDatabaseBackupService, DatabaseBackupService>();
        builder.Services.AddSingleton<IDatabaseErrorTranslator, DatabaseErrorTranslator>();
        builder.Services.AddSingleton(TimeProvider.System);
        builder.Services.AddScoped<IRegisterCompletenessService, RegisterCompletenessService>();
        builder.Services.AddScoped<IRegisterExportService, RegisterExportService>();
        
        builder.Services.AddScoped<ISelectListService, SelectListService>();
        builder.Services.AddScoped<IUnitService, UnitService>();
        builder.Services.AddScoped<IPropertyAllocationService, PropertyAllocationService>();
        builder.Services.AddScoped<IOwnershipService, OwnershipService>();
        builder.Services.AddScoped<IAgentService, AgentService>();
        builder.Services.AddAutoMapper(typeof(Program).Assembly);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        // Always use the friendly error surface. Technical exception details are
        // logged server-side and must never be rendered into a browser response.
        app.UseExceptionHandler("/Error");
        if (!app.Environment.IsDevelopment())
        {
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<BuildingContext>();
            try
            {
                var databasePath = new SqliteConnectionStringBuilder(resolvedConnection).DataSource;
                var databaseExisted = databasePath != ":memory:" && File.Exists(databasePath);
                if (databaseExisted && context.Database.GetPendingMigrations().Any())
                {
                    var backupService = services.GetRequiredService<IDatabaseBackupService>();
                    backupService.CreateAsync("pre-migration").GetAwaiter().GetResult();
                }
                DbInitializer.Initialize(
                    context,
                    builder.Configuration.GetValue("Database:SeedSampleData", false));
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }

        if (builder.Configuration.GetValue("DesktopDeployment:UseHttpsRedirection", true))
            app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapRazorPages()
           .WithStaticAssets();

        app.Run();
    }
}
