using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using RoeiJeRot.View.Wpf.Views.Windows;

namespace RoeiJeRot.View.Wpf.Logic
{
    /// <summary>
    /// Wrapper over `System.Windows.Window`.
    /// </summary>
    public class CustomWindow<T>  where T: UserControl, IEmbeddedScreen
    {
        private System.Windows.Window _window;
        private readonly Stack<T> _embeddedScreens;

        public CustomWindow()
        {
            _embeddedScreens = new Stack<T>();
        }

        /// <summary>
        /// Maximizes the current window.
        /// </summary>
        public void MaximizeWindow()
        {
            _window.WindowState = WindowState.Maximized;
        }

        /// <summary>
        /// Minimizes the current window.
        /// </summary>
        public void MinimizeWindow()
        {
            _window.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Swaps the current window with the given window.
        /// </summary>
        public void Swap(Window window)
        {
            _window = window;
        }

        /// <summary>
        /// Closes the current window.
        /// </summary>
        public void Close()
        {
            CloseAllScreens();

            _window?.Close();
        }

        /// <summary>
        /// Hides the previous window and shows the new window.
        /// </summary>
        public void ShowNew(Window window)
        {
            Close();
            Swap(window);
            _window?.Show();
        }

        /// <summary>
        /// Pushes an embedded screen into this screen.
        /// This screen will be the screen the users sees.
        /// </summary>
        /// <param name="screen"></param>
        public void PushEmbeddedScreen(T screen)
        {
            _embeddedScreens.Push(screen);
        }

        /// <summary>
        /// Closes the top screen.
        /// The previous window is shown when this screen is closed.
        /// </summary>
        public void CloseTopScreen()
        {
            if (_embeddedScreens.TryPop(out T screen))
            {
                screen.OnClose();
            }
        }

        /// <summary>
        /// Returns the top window.
        /// </summary>
        /// <returns></returns>
        public T TopScreen()
        {
            return _embeddedScreens.Peek();
        }

        /// <summary>
        /// Close all screens in the current window.
        /// </summary>
        public void CloseAllScreens()
        {
            while (_embeddedScreens.TryPop(out T screen))
            {
                screen.OnClose();
            }
        }
    }
}