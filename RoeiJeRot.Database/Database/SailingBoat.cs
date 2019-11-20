using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoeiJeRot.Database.Database
{
    [Table("sailing_boats")]
    public class SailingBoat
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public bool InService { get; set; }
        public int RequiredLevel { get; set; }

        public virtual ICollection<SailingReservation> SailingReservations { get; set; }
        public virtual ICollection<SailingBoatDamageReport> DamageReports { get; set; }
        public virtual ICollection<SailingCompetitionParticipant> SailingCompetitionParticipants { get; set; }
    }
}