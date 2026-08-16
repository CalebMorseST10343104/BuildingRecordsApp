using BuildingRecordsApp.Pages;
using BuildingRecordsApp.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace BuildingRecordsApp.Tests.Pages;

public class ErrorPageTests
{
    [Fact]
    public void Database_failure_is_translated_without_exposing_technical_details()
    {
        const string technical = "UNIQUE constraint failed: Vehicles.VehicleRegistration";
        var exception = new DbUpdateException("Save failed", new SqliteException(technical, 19, 2067));
        var model = ModelFor(exception);

        model.OnGet();

        Assert.Equal("That vehicle registration is already recorded.", model.UserMessage);
        Assert.DoesNotContain("constraint", model.UserMessage, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Unexpected_failure_uses_the_generic_safe_message()
    {
        const string technical = "Sensitive implementation detail";
        var model = ModelFor(new InvalidOperationException(technical));

        model.OnGet();

        Assert.Contains("Please review", model.UserMessage);
        Assert.DoesNotContain(technical, model.UserMessage);
    }

    private static ErrorModel ModelFor(Exception exception)
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Features.Set<IExceptionHandlerPathFeature>(new ExceptionHandlerFeature
        {
            Error = exception,
            Path = "/Vehicles/Create"
        });

        return new ErrorModel(NullLogger<ErrorModel>.Instance, new DatabaseErrorTranslator())
        {
            PageContext = new PageContext { HttpContext = httpContext }
        };
    }
}
