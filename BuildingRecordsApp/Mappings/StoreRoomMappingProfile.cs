using System;
using AutoMapper;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;

namespace BuildingRecordsApp.Mappings;

public class StoreRoomMappingProfile : Profile
{
    public StoreRoomMappingProfile()
    {
        CreateMap<StoreRoom, StoreRoomFormViewModel>().ReverseMap();
    }
}
