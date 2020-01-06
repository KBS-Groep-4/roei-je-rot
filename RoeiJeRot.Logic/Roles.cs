using System;
using System.Linq;

namespace RoeiJeRot.Logic
{
    /// <summary>
    ///     The permission a user can have.
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
    ///     Wrapper over the roles of Roei Je Rot.
    /// </summary>
    public class Roles
    {
        public const string ADMIN = "ADMIN";
        public const string MEMBER = "MEMBER";
        public const string WC = "WC";
        public const string MC = "MC";
        public const string STAFF = "STAFF";

        /// <summary>
        ///     Returns an `PermissionType` from the given permissions.
        /// </summary>
        /// <param name="permissions"></param>
        /// <returns></returns>
        public static PermissionType GetPermissionType(string[] permissions)
        {
            var permissionType = PermissionType.None;

            if (permissions.Contains(ADMIN)) permissionType |= PermissionType.Admin;
            if (permissions.Contains(MC)) permissionType |= PermissionType.Mc;
            if (permissions.Contains(WC)) permissionType |= PermissionType.Wc;
            if (permissions.Contains(MEMBER)) permissionType |= PermissionType.Member;
            if (permissions.Contains(STAFF)) permissionType |= PermissionType.Staff;

            return permissionType;
        }
    }
}