using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoeiJeRot.Database.Database
{
    [Table("boat_types")]
    public class BoatType
    {
        [Key] public int Id { get; set; }

        public int PossiblePassengers { get; set; }

        public int RequiredLevel { get; set; }

        public string Name { get; set; }

        public bool HasCommanderSeat { get; set; }
    }
}