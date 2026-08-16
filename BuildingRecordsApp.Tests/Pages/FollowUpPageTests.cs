using BuildingRecordsApp.Services;
using FollowUpIndexModel = BuildingRecordsApp.Pages.FollowUp.IndexModel;
using HomeIndexModel = BuildingRecordsApp.Pages.IndexModel;

namespace BuildingRecordsApp.Tests.Pages;

public class FollowUpPageTests
{
    [Fact]
    public async Task Home_page_reports_a_clear_state_when_there_are_no_issues()
    {
        var model = new HomeIndexModel(new StubCompletenessService([]));

        await model.OnGetAsync(CancellationToken.None);

        Assert.False(model.HasFollowUps);
        Assert.Equal(0, model.FollowUpCount);
        Assert.Equal(0, model.UrgentFollowUpCount);
    }

    [Fact]
    public async Task Home_page_counts_all_and_urgent_follow_ups()
    {
        var model = new HomeIndexModel(new StubCompletenessService([
            Issue("urgent", CompletenessSeverity.Urgent),
            Issue("important", CompletenessSeverity.Important)
        ]));

        await model.OnGetAsync(CancellationToken.None);

        Assert.True(model.HasFollowUps);
        Assert.Equal(2, model.FollowUpCount);
        Assert.Equal(1, model.UrgentFollowUpCount);
    }

    [Fact]
    public async Task Follow_up_page_filters_and_groups_unit_issues_with_urgent_first()
    {
        var unitImportant = Issue("important", CompletenessSeverity.Important, propertyId: 1, buildingId: 2, unitId: 3);
        var unitUrgent = Issue("urgent", CompletenessSeverity.Urgent, propertyId: 1, buildingId: 2, unitId: 3);
        var anotherProperty = Issue("elsewhere", CompletenessSeverity.Urgent, propertyId: 4, buildingId: 5, unitId: 6);
        var model = new FollowUpIndexModel(new StubCompletenessService([unitImportant, anotherProperty, unitUrgent]))
        {
            PropertyId = 1
        };

        await model.OnGetAsync(CancellationToken.None);

        Assert.Equal(3, model.TotalCount);
        Assert.Equal(2, model.FilteredCount);
        var group = Assert.Single(model.UnitGroups);
        Assert.Equal(3, group.UnitId);
        Assert.Equal(["urgent", "important"], group.Issues.Select(issue => issue.Code).ToArray());
        Assert.Empty(model.OtherIssues);
        Assert.Equal([2], model.Buildings.Select(option => option.Id).ToArray());
    }

    [Fact]
    public async Task Follow_up_page_keeps_non_unit_records_in_a_separate_section()
    {
        var model = new FollowUpIndexModel(new StubCompletenessService([
            Issue("property", CompletenessSeverity.Important, RegisterRecordType.Property, propertyId: 1)
        ]));

        await model.OnGetAsync(CancellationToken.None);

        Assert.Empty(model.UnitGroups);
        Assert.Single(model.OtherIssues);
        Assert.Equal(1, model.FilteredCount);
    }

    private static CompletenessIssue Issue(
        string code,
        CompletenessSeverity severity,
        RegisterRecordType recordType = RegisterRecordType.Unit,
        int? propertyId = null,
        int? buildingId = null,
        int? unitId = null) => new(
            code,
            severity,
            recordType,
            unitId ?? propertyId ?? 1,
            code,
            $"{code} summary",
            "/Units/Edit?id=1",
            propertyId,
            propertyId.HasValue ? $"Property {propertyId}" : null,
            buildingId,
            buildingId.HasValue ? $"Building {buildingId}" : null,
            unitId,
            unitId.HasValue ? $"{unitId}01" : null);

    private sealed class StubCompletenessService(IReadOnlyList<CompletenessIssue> issues) : IRegisterCompletenessService
    {
        public Task<IReadOnlyList<CompletenessIssue>> GetIssuesAsync(CancellationToken cancellationToken = default) =>
            Task.FromResult(issues);
    }
}
