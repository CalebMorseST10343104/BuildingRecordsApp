using Microsoft.EntityFrameworkCore;
using BuildingRecordsApp.Services;
using Microsoft.Data.Sqlite;
using Microsoft.VisualBasic;
using BuildingRecordsApp.Data;


internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        // Add services to the container.
        builder.Services.AddRazorPages();
        
        builder.Services.AddDbContext<BuildingContext>(options =>
        {
            var connectionString = builder.Configuration.GetConnectionString("BuildingContext");

            var connection = new SqliteConnection(connectionString);
            connection.Open();

            //Enable foreign keys explicitly
            var command = connection.CreateCommand();
            command.CommandText = "PRAGMA foreign_keys = ON;";
            command.ExecuteNonQuery();

            options.UseSqlite(connection);
        });
        
        builder.Services.AddScoped<ISelectListService, SelectListService>();


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<BuildingContext>();
            try
            {
                DbInitializer.Initialize(context);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapRazorPages()
           .WithStaticAssets();

        app.Run();
    }
}