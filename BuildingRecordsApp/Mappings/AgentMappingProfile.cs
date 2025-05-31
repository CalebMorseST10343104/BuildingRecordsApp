using System;
using AutoMapper;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using BuildingRecordsApp.Models.ItemViewModels;

namespace BuildingRecordsApp.Mappings;

public class AgentMappingProfile : Profile
{
    public AgentMappingProfile()
    {
        CreateMap<Agent, AgentFormViewModel>().ReverseMap();
        CreateMap<Agent, AgentItemViewModel>()
            .ForMember(
                dest => dest.CompanyName,
                opt => opt.MapFrom(src => src.AgentCompany != null ? src.AgentCompany.CompanyName : null));
    }
}
