using System;
using AutoMapper;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;

namespace BuildingRecordsApp.Mappings;

public class OwnershipMappingProfile : Profile
{
    public OwnershipMappingProfile()
    {
        CreateMap<Ownership, OwnershipFormViewModel>().ReverseMap();
    }
}
