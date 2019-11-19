using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoeiJeRot.Database.Database
{
    [Table("boats")]
    public class Boat
    {
        [Key]
        public int Id { get; set; }
    }
}