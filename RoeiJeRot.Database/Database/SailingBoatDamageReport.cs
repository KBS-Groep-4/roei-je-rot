using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoeiJeRot.Database.Database
{
    [Table("sailing_boat_damage_reports")]
    public class SailingBoatDamageReport
    {
        [Key] public int Id { get; set; }

        [ForeignKey(nameof(DamagedSailingBoat))]
        public int DamagedSailingBoatId { get; set; }

        [ForeignKey(nameof(DamagedBy))] public int DamagedById { get; set; }

        public DateTime DamagedAtDate { get; set; }
        public DateTime? DamageFixedDate { get; set; }

        public virtual User DamagedBy { get; set; }
        public virtual SailingBoat DamagedSailingBoat { get; set; }
    }
}