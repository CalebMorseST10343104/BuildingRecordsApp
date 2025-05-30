using System;
using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class OwnerItemViewModel
{
    public int? OwnerId { get; set; }

    public Dictionary<string, object?> OwnerDetails()
    {
        return new Dictionary<string, object?>
        {
            { nameof(OwnerId), OwnerId }
        };
    }
}
