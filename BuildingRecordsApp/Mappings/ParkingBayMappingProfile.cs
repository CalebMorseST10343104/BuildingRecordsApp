using System;
using AutoMapper;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;

namespace BuildingRecordsApp.Mappings;

public class ParkingBayMappingProfile : Profile
{
    public ParkingBayMappingProfile()
    {
        CreateMap<ParkingBay, ParkingBayFormViewModel>().ReverseMap();
    }
}
