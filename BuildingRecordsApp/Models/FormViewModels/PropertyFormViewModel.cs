using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models.FormViewModels;

public class PropertyFormViewModel
{
    public int? PropertyId { get; set; }

    [Required, Display(Name = "Property Name")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Address")]
    public string? Address { get; set; }
}
