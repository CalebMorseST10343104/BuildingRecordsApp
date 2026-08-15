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
        CreateMap<Agent, AgentItemViewEntry>()
            .ForMember(d => d.FirstName, o => o.MapFrom(s => s.Person.FirstName))
            .ForMember(d => d.LastName, o => o.MapFrom(s => s.Person.LastName))
            .ForMember(d => d.PhoneNumber, o => o.MapFrom(s => s.Person.PhoneNumber))
            .ForMember(d => d.Email, o => o.MapFrom(s => s.Person.Email))
            .ForMember(
                dest => dest.CompanyName,
                opt => opt.MapFrom(src => src.AgentCompany != null ? src.AgentCompany.CompanyName : null));
    }
}
