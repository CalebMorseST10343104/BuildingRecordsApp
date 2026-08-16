using BuildingRecordsApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace BuildingRecordsApp.Pages.System;

public sealed class IndexModel(
    IOptions<DesktopDeploymentOptions> options,
    IHostApplicationLifetime lifetime) : PageModel
{
    public IActionResult OnGet() => options.Value.AllowBrowserShutdown ? Page() : NotFound();

    public IActionResult OnPostShutdown()
    {
        if (!options.Value.AllowBrowserShutdown)
            return NotFound();

        Response.OnCompleted(() =>
        {
            _ = Task.Run(async () =>
            {
                await Task.Delay(500);
                lifetime.StopApplication();
            });
            return Task.CompletedTask;
        });
        return Page();
    }
}
