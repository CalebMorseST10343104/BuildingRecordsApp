using System;
using System.Reflection;
using BuildingRecordsApp.Attributes;
using BuildingRecordsApp.Enums;

namespace BuildingRecordsApp.Models.ItemViewModels;

public abstract class ItemViewModel
{
    public Dictionary<string, (object?, DisplayMode)> ToDictionary()
    {
        var dict = new Dictionary<string, (object?, DisplayMode)>();
        var type = GetType();

        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
        {
            var attr = prop.GetCustomAttributes(typeof(DisplayModeAttribute), false)
                           .FirstOrDefault() as DisplayModeAttribute;
            var displayMode = attr?.Mode ?? DisplayMode.Full;

            dict[prop.Name] = (prop.GetValue(this), displayMode);
        }

        return dict;
    }
}
