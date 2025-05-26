using System;
using AutoMapper;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;

namespace BuildingRecordsApp.Mappings;

public class OwnerMappingProfile : Profile
{
    public OwnerMappingProfile()
    {
        CreateMap<Owner, OwnerFormViewModel>().ReverseMap();
    }
}
