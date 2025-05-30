using System;
using AutoMapper;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using BuildingRecordsApp.Models.ItemViewModels;

namespace BuildingRecordsApp.Mappings;

public class OwnerMappingProfile : Profile
{
    public OwnerMappingProfile()
    {
        CreateMap<Owner, OwnerFormViewModel>().ReverseMap();
        CreateMap<Owner, OwnerItemViewModel>()
;    }
}
