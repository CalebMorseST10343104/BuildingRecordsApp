using System;
using AutoMapper;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using BuildingRecordsApp.Models.ItemViewModels;

namespace BuildingRecordsApp.Mappings;

public class OccupancyMappingProfile : Profile
{
    public OccupancyMappingProfile()
    {
        CreateMap<Occupancy, OccupancyFormViewModel>().ReverseMap();
        CreateMap<Occupancy, OccupancyItemViewModel>();
    }
}
