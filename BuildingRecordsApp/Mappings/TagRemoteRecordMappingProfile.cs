using System;
using AutoMapper;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;

namespace BuildingRecordsApp.Mappings;

public class TagRemoteRecordMappingProfile : Profile
{
    public TagRemoteRecordMappingProfile()
    {
        CreateMap<TagRemoteRecord, TagRemoteRecordFormViewModel>().ReverseMap();
    }
}
