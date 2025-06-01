using System;
using System.Reflection;
using BuildingRecordsApp.Attributes;
using BuildingRecordsApp.Enums;

namespace BuildingRecordsApp.Models.ItemViewModels;

public abstract class ItemViewModel : IDisplayEntry
{

    public List<string> GetHeaders(DisplayMode displayMode = DisplayMode.Basic)
    {
        // Returns a list of field names based on the display mode and includes headers of matching or lower display mode
        var headers = new List<string>();
        var type = GetType();
        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
        {
            if (prop.CanRead)
            {
                var displayAttribute = prop.GetCustomAttribute<DisplayModeAttribute>();
                if (displayAttribute != null && displayAttribute.Mode <= displayMode)
                {
                    headers.Add(prop.Name);
                }
            }
        }
        return headers;
    }
    public Dictionary<string, object?> GetValues(DisplayMode displayMode = DisplayMode.Basic)
    {
        // Returns a dictionary of field names and their values based on the display mode
        var values = new Dictionary<string, object?>();
        var type = GetType();
        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
        {
            if (prop.CanRead)
            {
                var displayAttribute = prop.GetCustomAttribute<DisplayModeAttribute>();
                if (displayAttribute != null && displayAttribute.Mode <= displayMode)
                {
                    var value = prop.GetValue(this);
                    values.Add(prop.Name, value);
                }
            }
        }
        return values;
    }

    public abstract string GetTitleHeader();

    public abstract bool IsTitleHeaderFieldName(object item);
}
