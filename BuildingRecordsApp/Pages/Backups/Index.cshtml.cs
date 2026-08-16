using BuildingRecordsApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BuildingRecordsApp.Pages.Backups;

public sealed class IndexModel(IDatabaseBackupService backupService, ILogger<IndexModel> logger) : PageModel
{
    public IReadOnlyList<DatabaseBackupInfo> Backups { get; private set; } = [];

    [TempData]
    public string? StatusMessage { get; set; }

    public void OnGet() => Backups = backupService.List();

    public async Task<IActionResult> OnPostCreateAsync()
    {
        try
        {
            var backup = await backupService.CreateAsync("manual", HttpContext.RequestAborted);
            StatusMessage = $"Backup created: {backup.FileName}";
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Could not create a database backup.");
            StatusMessage = "The backup could not be created. The register itself was not changed.";
        }

        return RedirectToPage();
    }

    public IActionResult OnGetDownload(string fileName)
    {
        var path = backupService.ResolveExistingBackup(fileName);
        return path is null
            ? NotFound()
            : PhysicalFile(path, "application/vnd.sqlite3", fileName);
    }
}
