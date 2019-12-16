using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoeiJeRot.Logic
{
    /// <summary>
    /// The permission a user can have.
    /// </summary>
    [Flags]
    public enum PermissionType
    {
        None = 0,
        Admin = 1,
        Member = 2,
        Mc = 4,
        Wc = 8,
        Staff = 16
    }

    /// <summary>
    /// Wrapper over the roles of Roei Je Rot.
    /// </summary>
    public class Roles
    {
        public const string ADMIN = "ADMIN";
        public const string MEMBER = "MEMBER";
        public const string WC = "WC";
        public const string MC = "MC";
        public const string STAFF = "STAFF";

        /// <summary>
        /// Returns an `PermissionType` from the given permissions.
        /// </summary>
        /// <param name="permissions"></param>
        /// <returns></returns>
        public static PermissionType GetPermissionType(string[] permissions)
        {
            PermissionType permissionType = PermissionType.None;

            if (permissions.Contains(Roles.ADMIN))
            {
                permissionType |= PermissionType.Admin;
            }
            if (permissions.Contains(Roles.MC))
            {
                permissionType |= PermissionType.Mc;
            }
            if (permissions.Contains(Roles.WC))
            {
                permissionType |= PermissionType.Wc;
            }
            if (permissions.Contains(Roles.MEMBER))
            {
                permissionType |= PermissionType.Member;
            }
            if (permissions.Contains(Roles.STAFF))
            {
                permissionType |= PermissionType.Staff;
            }

            return permissionType;
        }
    }
}
