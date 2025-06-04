using System;
using AutoMapper;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using BuildingRecordsApp.Models.ItemViewModels;


namespace BuildingRecordsApp.Mappings;

public class AgentCompanyMappingProfile : Profile
{
    public AgentCompanyMappingProfile()
    {
        CreateMap<AgentCompany, AgentCompanyFormViewModel>().ReverseMap();
        CreateMap<AgentCompany, AgentCompanyItemViewEntry>();
    }
}
