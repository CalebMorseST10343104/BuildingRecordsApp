using BuildingRecordsApp.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BuildingRecordsApp.Pages;

public class IndexModel : PageModel
{
    private readonly IRegisterCompletenessService _completenessService;

    public IndexModel(IRegisterCompletenessService completenessService)
    {
        _completenessService = completenessService;
    }

    public int FollowUpCount { get; private set; }
    public int UrgentFollowUpCount { get; private set; }
    public bool HasFollowUps => FollowUpCount > 0;

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        var issues = await _completenessService.GetIssuesAsync(cancellationToken);
        FollowUpCount = issues.Count;
        UrgentFollowUpCount = issues.Count(issue => issue.Severity == CompletenessSeverity.Urgent);
    }
}
