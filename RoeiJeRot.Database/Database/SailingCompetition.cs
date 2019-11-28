using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoeiJeRot.Database.Database
{
    [Table("sailing_competitions")]
    public class SailingCompetition
    {
        public SailingCompetition()
        {
            SailingCompetitionParticipants = new HashSet<SailingCompetitionParticipant>();
        }

        [Key] public int Id { get; set; }

        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [ForeignKey(nameof(SailingReservation))]
        public int ReservationId { get; set; }

        public virtual SailingReservation SailingReservation { get; set; }

        public virtual ICollection<SailingCompetitionParticipant> SailingCompetitionParticipants { get; set; }
    }
}