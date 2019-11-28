using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoeiJeRot.Database.Database
{
    [Table("sailing_reservations")]
    public class SailingReservation
    {
        [Key] public int Id { get; set; }

        public DateTime Date { get; set; }
        public TimeSpan Duration { get; set; }

        [ForeignKey(nameof(ReservedBy))] public int ReservedByUserId { get; set; }

        [ForeignKey(nameof(ReservedSailingBoat))]
        public int ReservedSailingBoatId { get; set; }

        public virtual User ReservedBy { get; set; }
        public virtual SailingBoat ReservedSailingBoat { get; set; }
    }
}