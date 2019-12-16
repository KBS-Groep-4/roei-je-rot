using System;
using RoeiJeRot.Logic;

namespace RoeiJeRot.View.Wpf.Logic
{
    /// <summary>
    /// The session of an user who is logged in.
    /// </summary>
    public class UserSession
    {
        public UserSession(int userId, string username, string email, string firstName, string lastName, PermissionType permissionType)
        {
            UserId = userId;
            Username = username;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            PermissionType = permissionType;
        }

        public int UserId { get; set; }
        public string Username  { get; set; }
        public string  Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        /// <summary>
        /// The permission of the user.
        /// </summary>
        public PermissionType  PermissionType { get; }
    }
}