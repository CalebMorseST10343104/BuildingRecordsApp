using System;
using AutoMapper;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using BuildingRecordsApp.Models.ItemViewModels;

namespace BuildingRecordsApp.Mappings;

public class CompanyTrustMappingProfile : Profile
{
    public CompanyTrustMappingProfile()
    {
        CreateMap<CompanyTrust, CompanyTrustFormViewModel>().ReverseMap();
        CreateMap<CompanyTrust, CompanyTrustItemViewEntry>();
    }
}
