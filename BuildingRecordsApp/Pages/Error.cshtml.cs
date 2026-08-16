using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Diagnostics;
using BuildingRecordsApp.Services;

namespace BuildingRecordsApp.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel : PageModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    public string UserMessage { get; private set; } = "We couldn't save that change. Please review the information and try again.";

    private readonly ILogger<ErrorModel> _logger;
    private readonly IDatabaseErrorTranslator _databaseErrors;

    public ErrorModel(ILogger<ErrorModel> logger, IDatabaseErrorTranslator databaseErrors)
    {
        _logger = logger;
        _databaseErrors = databaseErrors;
    }

    public void OnGet()
    {
        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        var exception = HttpContext.Features.Get<IExceptionHandlerPathFeature>()?.Error;
        var failedPath = HttpContext.Features.Get<IExceptionHandlerPathFeature>()?.Path;
        UserMessage = exception switch
        {
            BusinessRuleException rule => rule.Message,
            null => UserMessage,
            _ when ContainsDatabaseException(exception) => _databaseErrors.Translate(exception).UserMessage,
            _ => UserMessage
        };

        if (exception is not null)
            _logger.LogError(exception, "Request {RequestId} failed while processing {Path}", RequestId, failedPath);
    }

    private static bool ContainsDatabaseException(Exception exception)
    {
        for (Exception? current = exception; current is not null; current = current.InnerException)
        {
            if (current is Microsoft.EntityFrameworkCore.DbUpdateException or Microsoft.Data.Sqlite.SqliteException)
                return true;
        }

        return false;
    }
}

