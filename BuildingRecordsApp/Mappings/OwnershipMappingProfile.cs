using System;
using AutoMapper;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using BuildingRecordsApp.Models.ItemViewModels;

namespace BuildingRecordsApp.Mappings;

public class OwnershipMappingProfile : Profile
{
    public OwnershipMappingProfile()
    {
        CreateMap<Ownership, OwnershipFormViewModel>().ReverseMap();
        CreateMap<Ownership, OwnershipItemViewEntry>()
            .ForMember(
                dest => dest.BuildingName,
                opt => opt.MapFrom(src => src.Unit != null && src.Unit.Building != null ? src.Unit.Building.Name : null))
            .ForMember(
                dest => dest.UnitNumber,
                opt => opt.MapFrom(src => src.Unit != null ? src.Unit.UnitNumber : null))
            .ForMember(
                dest => dest.CompanyName,
                opt => opt.MapFrom(src => src.Organization != null ? src.Organization.Name : null));
    }
}
