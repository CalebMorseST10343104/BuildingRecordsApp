namespace BuildingRecordsApp.Tests.Pages;

public class CrudFormConventionTests
{
    private static readonly string[] RecordFolders =
    [
        "AccessDeviceCounts", "AgentCompanies", "Agents", "Buildings", "Leases",
        "Occupancies", "Organizations", "OwnershipContacts", "Ownerships", "ParkingBays",
        "Persons", "Properties", "StoreRooms", "Units", "Vehicles"
    ];

    [Fact]
    public void Every_shared_form_has_a_model_summary_and_a_prefixed_edit_id()
    {
        foreach (var folder in RecordFolders)
        {
            var source = File.ReadAllText(PagePath(folder, "_Form.cshtml"));

            Assert.Contains("asp-validation-summary=\"ModelOnly\"", source);
            Assert.Matches("<input asp-for=\"[A-Za-z]+Id\" type=\"hidden\" />", source);
            Assert.All(
                source.Split('\n').Where(line => line.Contains("<input ") && !line.Contains("type=\"hidden\"")),
                line => Assert.Contains("class=", line));
            Assert.All(
                source.Split('\n').Where(line => line.Contains("<select ")),
                line => Assert.Contains("class=\"form-select\"", line));
        }
    }

    [Fact]
    public void Every_create_and_edit_page_preserves_the_view_model_prefix()
    {
        foreach (var folder in RecordFolders)
        {
            foreach (var action in new[] { "Create", "Edit" })
            {
                var source = File.ReadAllText(PagePath(folder, $"{action}.cshtml"));

                Assert.Contains("<partial name=\"_Form\" for=\"ViewModel\" />", source);
                Assert.Contains("class=\"crud-form mt-4\"", source);
                Assert.Contains("_ValidationScriptsPartial", source);
                Assert.DoesNotContain("name=\"UnitId\"", source);
                Assert.DoesNotContain("name=\"PersonId\"", source);
            }
        }
    }

    [Fact]
    public void Every_form_uses_consistent_save_and_cancel_wording()
    {
        foreach (var folder in RecordFolders)
        {
            var create = File.ReadAllText(PagePath(folder, "Create.cshtml"));
            var edit = File.ReadAllText(PagePath(folder, "Edit.cshtml"));

            Assert.Contains(">Save new record</button>", create);
            Assert.Contains(">Save changes</button>", edit);
            Assert.Contains(">Cancel</a>", create);
            Assert.Contains(">Cancel</a>", edit);
        }
    }

    private static string PagePath(string folder, string fileName) =>
        Path.Combine(RepositoryRoot(), "BuildingRecordsApp", "Pages", folder, fileName);

    private static string RepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "buildingapp.sln")))
            directory = directory.Parent;

        return directory?.FullName ?? throw new DirectoryNotFoundException("Could not locate the buildingapp repository root.");
    }
}
