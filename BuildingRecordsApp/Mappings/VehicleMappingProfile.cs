using System;
using AutoMapper;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using BuildingRecordsApp.Models.ItemViewModels;

namespace BuildingRecordsApp.Mappings;

public class VehicleMappingProfile : Profile
{
    public VehicleMappingProfile()
    {
        CreateMap<Vehicle, VehicleFormViewModel>().ReverseMap();
        CreateMap<Vehicle, VehicleItemViewModel>()
            .ForMember(
                dest => dest.BuildingName,
                opt => opt.MapFrom(src => src.Unit != null && src.Unit.Building != null ? src.Unit.Building.Name : null))
            .ForMember(
                dest => dest.UnitNumber,
                opt => opt.MapFrom(src => src.Unit != null ? src.Unit.UnitNumber : null));
    }
}
