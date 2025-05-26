using System;
using AutoMapper;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;

namespace BuildingRecordsApp.Mappings;

public class CompanyTrustMappingProfile : Profile
{
    public CompanyTrustMappingProfile()
    {
        CreateMap<CompanyTrust, CompanyTrustFormViewModel>().ReverseMap();
    }
}
