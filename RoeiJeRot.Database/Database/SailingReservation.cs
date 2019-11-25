using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoeiJeRot.Database.Database
{
    [Table("sailing_reservations")]
    public class SailingReservation
    {
        public bool availableOn(DateTime start, TimeSpan duration)
        {
            var thisStart = Date;
            var thisEnd = Date + TimeSpan.FromMinutes(Duration);

            var end = Date + duration;

            return thisEnd < start || thisStart > end;
        }

        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; }
        public byte Duration { get; set; }

        [ForeignKey(nameof(ReservedBy))]
        public int ReservedByUserId { get; set; }
        [ForeignKey(nameof(ReservedSailingBoat))]
        public int ReservedSailingBoatId { get; set; }

        public virtual User ReservedBy { get; set; }
        public virtual SailingBoat ReservedSailingBoat { get; set; }
    }
}