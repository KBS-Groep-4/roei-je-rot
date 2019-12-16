using System.ComponentModel.DataAnnotations.Schema;

namespace RoeiJeRot.Database.Database
{
    [Table("permissions")]
    public class Permission
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}