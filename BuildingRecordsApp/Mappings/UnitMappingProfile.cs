using System;
using AutoMapper;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;

namespace BuildingRecordsApp.Mappings;

public class UnitMappingProfile : Profile
{
    public UnitMappingProfile()
    {
        CreateMap<Unit, UnitFormViewModel>().ReverseMap();
    }
}
