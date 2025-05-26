using System;
using AutoMapper;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;


namespace BuildingRecordsApp.Mappings;

public class AgentCompanyMappingProfile : Profile
{
    public AgentCompanyMappingProfile()
    {
        CreateMap<AgentCompany, AgentCompanyFormViewModel>().ReverseMap();
    }
}
