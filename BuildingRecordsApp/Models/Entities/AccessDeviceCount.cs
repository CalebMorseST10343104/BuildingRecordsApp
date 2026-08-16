using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models.Entities
{
    public class AccessDeviceCount
    {
        public int AccessDeviceCountId { get; set; }

        [Display(Name = "Ownership contact tags")]
        [Range(0, int.MaxValue)]
        public int? OwnershipContactTagCount { get; set; }

        [Display(Name = "Ownership contact remotes")]
        [Range(0, int.MaxValue)]
        public int? OwnershipContactRemoteCount { get; set; }

        [Display(Name = "Occupant tags")]
        [Range(0, int.MaxValue)]
        public int? OccupantTagCount { get; set; }

        [Display(Name = "Occupant remotes")]
        [Range(0, int.MaxValue)]
        public int? OccupantRemoteCount { get; set; }

        [Display(Name = "Agent tags")]
        [Range(0, int.MaxValue)]
        public int? AgentTagCount { get; set; }

        [Display(Name = "Agent remotes")]
        [Range(0, int.MaxValue)]
        public int? AgentRemoteCount { get; set; }

        public Unit? Unit { get; set; } // Navigation property

        public int UnitId { get; set; }
    }
}
