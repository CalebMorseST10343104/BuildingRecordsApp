using System.ComponentModel.DataAnnotations;

namespace BuildingRecordsApp.Models
{
    public class TagRemoteRecord
    {
        public int TagRemoteRecordId { get; set; }

        [Display(Name = "Tags Owner")]
        public int TagsOwner { get; set; }

        [Display(Name = "Remotes Owner")]
        public int RemotesOwner { get; set; }

        [Display(Name = "Tags Occupant")]
        public int TagsOccupant { get; set; }

        [Display(Name = "Remotes Occupant")]
        public int RemotesOccupant { get; set; }

        [Display(Name = "Tags Agent")]
        public int TagsAgent { get; set; }

        [Display(Name = "Remotes Agent")]
        public int RemotesAgent { get; set; }

        public Unit? Unit { get; set; } // Navigation property

        public int? UnitId { get; set; } // Foreign key
    }
}