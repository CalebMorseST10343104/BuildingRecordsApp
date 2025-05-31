using System;

namespace BuildingRecordsApp.Models.ItemViewModels;

public interface IItemViewModel
{
    bool HasTitleHeader();

    string GetTitleHeader(string valueIfNull);

    string GetTitleHeaderFieldName(bool formatted = false);
}
