using BuildingRecordsApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BuildingRecordsApp.Pages.FollowUp;

public class IndexModel : PageModel
{
    private readonly IRegisterCompletenessService _completenessService;

    public IndexModel(IRegisterCompletenessService completenessService)
    {
        _completenessService = completenessService;
    }

    [BindProperty(SupportsGet = true)]
    public int? PropertyId { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? BuildingId { get; set; }

    [BindProperty(SupportsGet = true)]
    public RegisterRecordType? RecordType { get; set; }

    [BindProperty(SupportsGet = true)]
    public CompletenessSeverity? Severity { get; set; }

    public int TotalCount { get; private set; }
    public int UrgentCount { get; private set; }
    public int ImportantCount { get; private set; }
    public IReadOnlyList<FilterOption> Properties { get; private set; } = [];
    public IReadOnlyList<FilterOption> Buildings { get; private set; } = [];
    public IReadOnlyList<UnitIssueGroup> UnitGroups { get; private set; } = [];
    public IReadOnlyList<CompletenessIssue> OtherIssues { get; private set; } = [];
    public int FilteredCount => UnitGroups.Sum(group => group.Issues.Count) + OtherIssues.Count;
    public bool FiltersApplied => PropertyId.HasValue || BuildingId.HasValue || RecordType.HasValue || Severity.HasValue;

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        var allIssues = await _completenessService.GetIssuesAsync(cancellationToken);
        TotalCount = allIssues.Count;
        UrgentCount = allIssues.Count(issue => issue.Severity == CompletenessSeverity.Urgent);
        ImportantCount = TotalCount - UrgentCount;

        Properties = allIssues
            .Where(issue => issue.PropertyId.HasValue)
            .GroupBy(issue => issue.PropertyId!.Value)
            .Select(group => new FilterOption(group.Key, group.Select(issue => issue.PropertyName).FirstOrDefault(name => !string.IsNullOrWhiteSpace(name)) ?? $"Property {group.Key}"))
            .OrderBy(option => option.Label)
            .ToList();

        Buildings = allIssues
            .Where(issue => issue.BuildingId.HasValue && (!PropertyId.HasValue || issue.PropertyId == PropertyId))
            .GroupBy(issue => issue.BuildingId!.Value)
            .Select(group => new FilterOption(group.Key, group.Select(issue => issue.BuildingName).FirstOrDefault(name => !string.IsNullOrWhiteSpace(name)) ?? $"Building {group.Key}"))
            .OrderBy(option => option.Label)
            .ToList();

        var filtered = allIssues
            .Where(issue => !PropertyId.HasValue || issue.PropertyId == PropertyId)
            .Where(issue => !BuildingId.HasValue || issue.BuildingId == BuildingId)
            .Where(issue => !RecordType.HasValue || issue.RecordType == RecordType)
            .Where(issue => !Severity.HasValue || issue.Severity == Severity)
            .ToList();

        UnitGroups = filtered
            .Where(issue => issue.UnitId.HasValue)
            .GroupBy(issue => issue.UnitId!.Value)
            .Select(group => new UnitIssueGroup(
                group.Key,
                group.Select(issue => issue.UnitNumber).FirstOrDefault(number => !string.IsNullOrWhiteSpace(number)) ?? $"Unit {group.Key}",
                group.Select(issue => issue.PropertyName).FirstOrDefault(name => !string.IsNullOrWhiteSpace(name)),
                group.Select(issue => issue.BuildingName).FirstOrDefault(name => !string.IsNullOrWhiteSpace(name)),
                group.OrderBy(issue => issue.Severity).ThenBy(issue => issue.RecordType).ThenBy(issue => issue.Summary).ToList()))
            .OrderBy(group => group.PropertyName)
            .ThenBy(group => group.BuildingName)
            .ThenBy(group => group.UnitNumber)
            .ToList();

        OtherIssues = filtered
            .Where(issue => !issue.UnitId.HasValue)
            .OrderBy(issue => issue.Severity)
            .ThenBy(issue => issue.RecordType)
            .ThenBy(issue => issue.RecordLabel)
            .ToList();
    }

    public sealed record FilterOption(int Id, string Label);
    public sealed record UnitIssueGroup(int UnitId, string UnitNumber, string? PropertyName, string? BuildingName, IReadOnlyList<CompletenessIssue> Issues);
}
