using System.ComponentModel.DataAnnotations.Schema;

namespace RoeiJeRot.Database.Database
{
    [Table("user_permissions")]
    public class UserPermission
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Permission))] public int PermissionId { get; set; }
        [ForeignKey(nameof(User))] public int UserId { get; set; }

        public virtual Permission Permission { get; set; }
        public virtual User User { get; set; }
    }
}