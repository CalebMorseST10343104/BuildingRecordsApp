using System;
using AutoMapper;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;

namespace BuildingRecordsApp.Mappings;

public class LeaseMappingProfile : Profile
{
    public LeaseMappingProfile()
    {
        CreateMap<Lease, LeaseFormViewModel>().ReverseMap();
    }
}