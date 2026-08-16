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
        CreateMap<Unit, UnitItemViewEntry>()
            .ForMember(dest => dest.PropertyName, opt => opt.MapFrom(src => src.Building != null && src.Building.Property != null ? src.Building.Property.Name : null))
            .ForMember(
                dest => dest.BuildingName,
                opt => opt.MapFrom(src => src.Building != null ? src.Building.Name : null));
    }
}
