using System;
using BuildingRecordsApp.Enums;

namespace BuildingRecordsApp.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class DisplayModeAttribute : Attribute
{
    public DisplayMode Mode { get; }

    public DisplayModeAttribute(DisplayMode mode)
    {
        Mode = mode;
    }

    public DisplayModeAttribute(string mode)
    {
        Mode = mode.ToLower() switch
        {
            "basic" => DisplayMode.Basic,
            "detailed" => DisplayMode.Detailed,
            "full" => DisplayMode.Full,
            _ => throw new ArgumentException($"Invalid display mode: {mode}"),
        };
    }
}
