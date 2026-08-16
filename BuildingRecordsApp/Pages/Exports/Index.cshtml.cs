using BuildingRecordsApp.Data;
using BuildingRecordsApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Pages.Exports;

public sealed class IndexModel(BuildingContext context, IRegisterExportService exportService, ILogger<IndexModel> logger) : PageModel
{
    [BindProperty, Required(ErrorMessage = "Select a property to export.")]
    public int? PropertyId { get; set; }

    [BindProperty]
    public List<int> BuildingIds { get; set; } = [];

    public List<SelectListItem> Properties { get; private set; } = [];
    public List<ExportBuildingOption> Buildings { get; private set; } = [];

    public async Task OnGetAsync() => await LoadOptionsAsync();

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadOptionsAsync();
            return Page();
        }

        try
        {
            var export = await exportService.ExportExcelAsync(PropertyId!.Value, BuildingIds, HttpContext.RequestAborted);
            return File(export.Content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", export.FileName);
        }
        catch (ArgumentException exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Could not export the register for property {PropertyId}.", PropertyId);
            ModelState.AddModelError(string.Empty, "The Excel workbook could not be created. The register itself was not changed.");
        }

        await LoadOptionsAsync();
        return Page();
    }

    private async Task LoadOptionsAsync()
    {
        Properties = await context.Properties.AsNoTracking().OrderBy(item => item.Name)
            .Select(item => new SelectListItem(item.Name, item.PropertyId.ToString())).ToListAsync();
        Buildings = await context.Buildings.AsNoTracking().OrderBy(item => item.Property.Name).ThenBy(item => item.Name)
            .Select(item => new ExportBuildingOption(item.BuildingId, item.PropertyId, item.Name)).ToListAsync();
    }
}

public sealed record ExportBuildingOption(int BuildingId, int PropertyId, string Name);
