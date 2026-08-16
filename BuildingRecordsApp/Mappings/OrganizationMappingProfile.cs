using System;
using AutoMapper;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using BuildingRecordsApp.Models.ItemViewModels;

namespace BuildingRecordsApp.Mappings;

public class OrganizationMappingProfile : Profile
{
    public OrganizationMappingProfile()
    {
        CreateMap<Organization, OrganizationFormViewModel>()
            .ForMember(dest => dest.RegistrationNumber, opt => opt.MapFrom(src => src.RegistrationReference))
            .ReverseMap()
            .ForMember(dest => dest.RegistrationReference, opt => opt.MapFrom(src => src.RegistrationNumber));
        CreateMap<Organization, OrganizationItemViewEntry>()
            .ForMember(dest => dest.RegistrationNumber, opt => opt.MapFrom(src => src.RegistrationReference));
    }
}
