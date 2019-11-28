using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoeiJeRot.Database.Database
{
    [Table("sailing_competition_participants")]
    public class SailingCompetitionParticipant
    {
        [Key] public int Id { get; set; }

        [ForeignKey(nameof(SailingCompetition))]
        public int SailingCompetitionId { get; set; }

        [ForeignKey(nameof(SailingParticipant))]
        public int ParticipantId { get; set; }

        public virtual SailingCompetition SailingCompetition { get; set; }
        public virtual User SailingParticipant { get; set; }
    }
}