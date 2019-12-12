using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoeiJeRot.Database.Database
{
    [Table("users")]
    public class User
    {
        public User()
        {
            Reservations = new HashSet<SailingReservation>();
            DamageReports = new HashSet<SailingBoatDamageReport>();
            SailingCompetitionParticipants = new HashSet<SailingCompetitionParticipant>();
        }

        [Key] public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int SailingLevel { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string  Email { get; set; }  

        public virtual ICollection<SailingReservation> Reservations { get; set; }
        public virtual ICollection<SailingBoatDamageReport> DamageReports { get; set; }

        public virtual ICollection<SailingCompetitionParticipant> SailingCompetitionParticipants { get; set; }
    }
}