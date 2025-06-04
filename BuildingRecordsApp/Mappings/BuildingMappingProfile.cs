using System;
using AutoMapper;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using BuildingRecordsApp.Models.ItemViewModels;

namespace BuildingRecordsApp.Mappings;

public class BuildingMappingProfile : Profile
{
    public BuildingMappingProfile()
    {
        CreateMap<Building, BuildingFormViewModel>().ReverseMap();
        CreateMap<Building, BuildingItemViewEntry>();
    }
}
