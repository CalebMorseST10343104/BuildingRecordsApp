using System;
using AutoMapper;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using BuildingRecordsApp.Models.ItemViewModels;

namespace BuildingRecordsApp.Mappings;

public class LeaseMappingProfile : Profile
{
    public LeaseMappingProfile()
    {
        CreateMap<Lease, LeaseFormViewModel>().ReverseMap();
        CreateMap<Lease, LeaseItemViewModel>()
            .ForMember(
                dest => dest.UnitNumber,
                opt => opt.MapFrom(src => src.Unit != null ? src.Unit.UnitNumber : null))
            .ForMember(
                dest => dest.BuildingName,
                opt => opt.MapFrom(src => src.Unit != null && src.Unit.Building != null ? src.Unit.Building.Name : null));
    }
}