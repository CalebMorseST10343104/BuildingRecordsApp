using System;
using AutoMapper;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;

namespace BuildingRecordsApp.Mappings;

public class VehicleMappingProfile : Profile
{
    public VehicleMappingProfile()
    {
        CreateMap<Vehicle, VehicleFormViewModel>().ReverseMap();
    }
}
