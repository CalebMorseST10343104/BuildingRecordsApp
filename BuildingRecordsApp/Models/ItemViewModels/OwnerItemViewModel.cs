using System;
using System.ComponentModel.DataAnnotations;
using BuildingRecordsApp.Attributes;

namespace BuildingRecordsApp.Models.ItemViewModels;

public class OwnerItemViewModel : ItemViewModel
{
    public int? OwnerId { get; set; }
}
