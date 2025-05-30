using System;
using AutoMapper;
using BuildingRecordsApp.Models.Entities;
using BuildingRecordsApp.Models.FormViewModels;
using BuildingRecordsApp.Models.ItemViewModels;

namespace BuildingRecordsApp.Mappings;

public class TagRemoteRecordMappingProfile : Profile
{
    public TagRemoteRecordMappingProfile()
    {
        CreateMap<TagRemoteRecord, TagRemoteRecordFormViewModel>().ReverseMap();
        CreateMap<TagRemoteRecord, TagRemoteRecordItemViewModel>();
    }
}
