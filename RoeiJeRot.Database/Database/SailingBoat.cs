using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoeiJeRot.Database.Database
{
    [Table("sailing_boats")]
    public class SailingBoat
    {
        public SailingBoat()
        {
            SailingReservations = new HashSet<SailingReservation>();
            DamageReports = new HashSet<SailingBoatDamageReport>();
        }

        [Key] public int Id { get; set; }

        public int Status { get; set; }

        [ForeignKey(nameof(BoatType))] public int BoatTypeId { get; set; }

        public virtual BoatType BoatType { get; set; }

        public virtual ICollection<SailingReservation> SailingReservations { get; set; }
        public virtual ICollection<SailingBoatDamageReport> DamageReports { get; set; }
    }
}