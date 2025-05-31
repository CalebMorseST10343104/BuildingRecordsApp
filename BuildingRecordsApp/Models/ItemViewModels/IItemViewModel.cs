using System;

namespace BuildingRecordsApp.Models.ItemViewModels;

public interface IItemViewModel
{
    string GetTitleHeader();

    bool IsTitleHeaderFieldName(object item);
}
