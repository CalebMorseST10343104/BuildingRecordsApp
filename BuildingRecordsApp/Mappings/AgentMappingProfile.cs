using System;
using AutoMapper;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;

namespace BuildingRecordsApp.Mappings;

public class AgentMappingProfile : Profile
{
    public AgentMappingProfile()
    {
        CreateMap<Agent, AgentFormViewModel>().ReverseMap();
    }
}
