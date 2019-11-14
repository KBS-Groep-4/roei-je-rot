using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace RoeiJeRot.Database.Database
{
    public class Boat
    {
        [Key]
        public int Id { get; set; }
    }
}