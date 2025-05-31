using System;
using AutoMapper;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using BuildingRecordsApp.Models.ItemViewModels;

namespace BuildingRecordsApp.Mappings;

public class OwnerMappingProfile : Profile
{
    public OwnerMappingProfile()
    {
        CreateMap<Owner, OwnerFormViewModel>().ReverseMap();
        CreateMap<Owner, OwnerItemViewModel>()
            .ForMember(
                dest => dest.FirstName,
                opt => opt.MapFrom(src => src.Person != null ? src.Person.FirstName : null))
            .ForMember(
                dest => dest.LastName,
                opt => opt.MapFrom(src => src.Person != null ? src.Person.LastName : null))
            .ForMember(
                dest => dest.BuildingName,
                opt => opt.MapFrom(src => src.Ownership != null && src.Ownership.Unit != null ? src.Ownership.Unit.UnitNumber : null))
            .ForMember(
                dest => dest.UnitNumber,
                opt => opt.MapFrom(src => src.Ownership != null && src.Ownership.Unit != null && src.Ownership.Unit.Building != null ? src.Ownership.Unit.Building.Name : null));
    }
}
