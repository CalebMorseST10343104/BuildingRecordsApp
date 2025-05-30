using System;
using AutoMapper;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using BuildingRecordsApp.Models.ItemViewModels;

namespace BuildingRecordsApp.Mappings;

public class LeaseMappingProfile : Profile
{
    public LeaseMappingProfile()
    {
        CreateMap<Lease, LeaseFormViewModel>().ReverseMap();
        CreateMap<Lease, LeaseItemViewModel>();
    }
}