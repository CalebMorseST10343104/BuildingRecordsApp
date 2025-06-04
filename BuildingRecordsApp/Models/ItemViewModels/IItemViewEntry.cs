using System;
using BuildingRecordsApp.Enums;

namespace BuildingRecordsApp.Models.ItemViewModels;

public interface IItemViewEntry
{
    public List<string> GetHeaders(DisplayMode displayMode);

    public Dictionary<string, object?> GetValues(DisplayMode displayMode);

    string GetTitleHeader();

    bool IsTitleHeaderFieldName(object item);

    int GetId();
}
