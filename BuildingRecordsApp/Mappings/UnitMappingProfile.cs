using System;
using AutoMapper;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using BuildingRecordsApp.Models.ItemViewModels;

namespace BuildingRecordsApp.Mappings;

public class UnitMappingProfile : Profile
{
    public UnitMappingProfile()
    {
        CreateMap<Unit, UnitFormViewModel>().ReverseMap();
        CreateMap<Unit, UnitItemViewModel>()
            .ForMember(
                dest => dest.BuildingName,
                opt => opt.MapFrom(src => src.Building != null ? src.Building.Name : null));
    }
}
