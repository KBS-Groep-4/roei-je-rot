using System.Collections.Generic;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RoeiJeRot.Logic.Services;
using RoeiJeRot.View.Wpf.ViewModels;
using RoeiJeRot.View.Wpf.Views.Windows;

namespace RoeiJeRot.View.Wpf.Logic
{
    /// <summary>
    /// Manager that
    /// - Interacts between the data supplier and UI.
    /// - Switches between windows
    /// - Owner of current user and current window.
    /// </summary>
    public class WindowManager
    {
        public CustomWindow<CustomUserControl> CurrentWindow { get; }

        /// <summary>
        /// DI environment that manages instantiating services.
        /// </summary>
        private readonly IHost _host;
        
        /// <summary>
        /// The current authenticated username.
        /// </summary>
        public string CurrentUserName { get; private set; }
        
        public WindowManager(IHost host)
        {
            _host = host;
            CurrentWindow = new CustomWindow<CustomUserControl>();
        }

        /// <summary>
        /// Log out and switch back to the login window.
        /// </summary>
        public void Logout()
        {
            ShowLogin();
        }

        /// <summary>
        /// Show the login window.
        /// </summary>
        public void ShowLogin()
        {
            var window = GetWindow<LoginWindow>();
            CurrentWindow.ShowNew(window);
        }
        
        /// <summary>
        /// Show the main window if the given username and password are correct.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool ShowMainWindow(string username, string password)
        {
            var authenticationService = GetService<IAuthenticationService>();
            
            if (authenticationService.AuthenticateUser(username, password))
            {
                CurrentUserName = username;
                CurrentWindow.ShowNew(GetWindow<MainWindow>());
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns an new instance of the given window.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private System.Windows.Window GetWindow<T>() where T: System.Windows.Window
        {
            return (System.Windows.Window)_host.Services.GetService<T>();
        }

        /// <summary>
        /// Returns an new instance of the given service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private T GetService<T>()
        {
            return _host.Services.GetService<T>();
        }
    }
}
