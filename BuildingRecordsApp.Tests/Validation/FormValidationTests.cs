using System.ComponentModel.DataAnnotations;
using BuildingRecordsApp.Models.FormViewModels;

namespace BuildingRecordsApp.Tests.Validation;

public class FormValidationTests
{
    [Fact]
    public void Lease_end_date_cannot_precede_start_date()
    {
        var model = new LeaseFormViewModel
        {
            StartDate = new DateTime(2026, 8, 15),
            EndDate = new DateTime(2026, 8, 14)
        };

        Assert.Contains(Validate(model), error => error.MemberNames.Contains(nameof(model.EndDate)));
    }

    [Fact]
    public void Access_device_counts_cannot_be_negative()
    {
        var model = new AccessDeviceCountFormViewModel
        {
            UnitId = 1,
            OwnershipContactTagCount = -1
        };

        Assert.Contains(Validate(model), error => error.MemberNames.Contains(nameof(model.OwnershipContactTagCount)));
    }

    [Fact]
    public void Required_register_identifiers_are_validated()
    {
        Assert.NotEmpty(Validate(new UnitFormViewModel()));
        Assert.NotEmpty(Validate(new VehicleFormViewModel()));
        Assert.NotEmpty(Validate(new OccupancyFormViewModel()));
    }

    private static List<ValidationResult> Validate(object model)
    {
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(model, new ValidationContext(model), results, validateAllProperties: true);
        return results;
    }
}
