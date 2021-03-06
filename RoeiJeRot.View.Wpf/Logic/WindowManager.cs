﻿using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RoeiJeRot.Logic;
using RoeiJeRot.Logic.Services;
using RoeiJeRot.View.Wpf.Views.Windows;

namespace RoeiJeRot.View.Wpf.Logic
{
    /// <summary>
    ///     Manager that
    ///     - Interacts between the data supplier and UI.
    ///     - Switches between windows
    ///     - Owner of current user and current window.
    /// </summary>
    public class WindowManager
    {
        /// <summary>
        ///     DI environment that manages instantiating services.
        /// </summary>
        private readonly IHost _host;

        public WindowManager(IHost host)
        {
            _host = host;
            CurrentWindow = new CustomWindow<CustomUserControl>();
        }

        /// <summary>
        ///     The current active window.
        /// </summary>
        public CustomWindow<CustomUserControl> CurrentWindow { get; }

        /// <summary>
        ///     The current authenticated username.
        /// </summary>
        public UserSession UserSession { get; private set; }

        /// <summary>
        ///     Log out and switch back to the login window.
        /// </summary>
        public void Logout()
        {
            UserSession = null;
            ShowLogin();
        }

        /// <summary>
        ///     Show the login window.
        /// </summary>
        public void ShowLogin()
        {
            var window = GetWindow<LoginWindow>();
            CurrentWindow.ShowNew(window);
        }

        /// <summary>
        ///     Show the main window if the given username and password are correct.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool ShowMainWindow(string username, string password)
        {
            var authenticationService = GetService<IAuthenticationService>();
            var userService = GetService<IUserService>();

            if (authenticationService.AuthenticateUser(username, password))
            {
                var user = userService.GetUserByUserName(username);

                var permissions = userService.GetUserPermissions(user.Id);
                var permissionType = Roles.GetPermissionType(permissions);

                UserSession = new UserSession(user.Id, user.Username, user.Email, user.FirstName, user.LastName,
                    permissionType);

                CurrentWindow.ShowNew(GetWindow<MainWindow>());
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Returns an new instance of the given window.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private Window GetWindow<T>() where T : Window
        {
            return _host.Services.GetService<T>();
        }

        /// <summary>
        ///     Returns an new instance of the given service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetService<T>()
        {
            return _host.Services.GetService<T>();
        }
    }
}