using System;
using RoeiJeRot.Logic;

namespace RoeiJeRot.View.Wpf.Logic
{
    /// <summary>
    /// The session of an user who is logged in.
    /// </summary>
    public class UserSession
    {
        public UserSession(string username, string email, string firstName, string lastName, PermissionType permissionType)
        {
            Username = username;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            PermissionType = permissionType;
        }

        public string Username  { get;  }
        public string  Email { get; }
        public string FirstName { get; }
        public string LastName { get; }

        /// <summary>
        /// The permission of the user.
        /// </summary>
        public PermissionType  PermissionType { get; }
    }


}