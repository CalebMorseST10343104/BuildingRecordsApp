namespace BuildingRecordsApp.Services;

public sealed class DesktopDeploymentOptions
{
    public bool UseHttpsRedirection { get; set; } = true;
    public bool AllowBrowserShutdown { get; set; }
}
