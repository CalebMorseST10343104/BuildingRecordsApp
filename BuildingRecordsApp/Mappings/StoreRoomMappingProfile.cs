using System;
using AutoMapper;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using BuildingRecordsApp.Models.ItemViewModels;

namespace BuildingRecordsApp.Mappings;

public class StoreRoomMappingProfile : Profile
{
    public StoreRoomMappingProfile()
    {
        CreateMap<StoreRoom, StoreRoomFormViewModel>().ReverseMap();
        CreateMap<StoreRoom, StoreRoomItemViewModel>();
    }
}
