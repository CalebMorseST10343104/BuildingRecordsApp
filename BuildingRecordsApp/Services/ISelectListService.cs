using BuildingRecordsApp.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

public interface ISelectListService
{
    Task<SelectList> GetUnitSelectListAsync();
    Task<SelectList> GetUnitSelectListAsync(UsageContext usageContext);
    Task<SelectList> GetBuildingSelectListAsync();
    Task<SelectList> GetParkingBaySelectListAsync();
    Task<SelectList> GetAgentCompanySelectListAsync();
    Task<SelectList> GetAgentSelectListAsync();
    Task<SelectList> GetPersonSelectListAsync();
    Task<SelectList> GetOwnershipSelectListAsync();
    Task<SelectList> GetCompanyTrustSelectListAsync();
}
