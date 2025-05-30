using System;
using AutoMapper;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using BuildingRecordsApp.Models.ItemViewModels;

namespace BuildingRecordsApp.Mappings;

public class OwnershipMappingProfile : Profile
{
    public OwnershipMappingProfile()
    {
        CreateMap<Ownership, OwnershipFormViewModel>().ReverseMap();
        CreateMap<Ownership, OwnershipItemViewModel>();
    }
}
