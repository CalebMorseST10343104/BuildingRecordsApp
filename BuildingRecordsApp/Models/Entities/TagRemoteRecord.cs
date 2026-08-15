using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models.Entities
{
    public class TagRemoteRecord
    {
        public int TagRemoteRecordId { get; set; }

        [Display(Name = "Tags OwnershipContact")]
        [Range(0, int.MaxValue)]
        public int? TagsOwner { get; set; }

        [Display(Name = "Remotes OwnershipContact")]
        [Range(0, int.MaxValue)]
        public int? RemotesOwner { get; set; }

        [Display(Name = "Tags Occupant")]
        [Range(0, int.MaxValue)]
        public int? TagsOccupant { get; set; }

        [Display(Name = "Remotes Occupant")]
        [Range(0, int.MaxValue)]
        public int? RemotesOccupant { get; set; }

        [Display(Name = "Tags Agent")]
        [Range(0, int.MaxValue)]
        public int? TagsAgent { get; set; }

        [Display(Name = "Remotes Agent")]
        [Range(0, int.MaxValue)]
        public int? RemotesAgent { get; set; }

        public Unit? Unit { get; set; } // Navigation property

        public int UnitId { get; set; }
    }
}
