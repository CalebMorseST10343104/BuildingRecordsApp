using System;
using AutoMapper;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using BuildingRecordsApp.Models.ItemViewModels;

namespace BuildingRecordsApp.Mappings;

public class OccupancyMappingProfile : Profile
{
    public OccupancyMappingProfile()
    {
        CreateMap<Occupancy, OccupancyFormViewModel>().ReverseMap();
        CreateMap<Occupancy, OccupancyItemViewModel>()
            .ForMember(
                dest => dest.OccupantFirstName,
                opt => opt.MapFrom(src => src.Occupant != null ? src.Occupant.FirstName : null))
            .ForMember(
                dest => dest.OccupantLastName,
                opt => opt.MapFrom(src => src.Occupant != null ? src.Occupant.LastName : null))
            .ForMember(
                dest => dest.BuildingName,
                opt => opt.MapFrom(src => src.Unit != null && src.Unit.Building != null ? src.Unit.Building.Name : null))
            .ForMember(
                dest => dest.UnitNumber,
                opt => opt.MapFrom(src => src.Unit != null ? src.Unit.UnitNumber : null));
    }
}
