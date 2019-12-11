﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using RoeiJeRot.View.Wpf.Views.Windows;

namespace RoeiJeRot.View.Wpf.Logic
{
    public class CustomUserControl : UserControl, IEmbeddedScreen
    {
        public void OnClose()
        {
        }
    }

    /// <summary>
    /// Wrapper over `Window`.
    /// </summary>
    public class CustomWindow<T> where T: UserControl, IEmbeddedScreen
    {
        private System.Windows.Window _window;
        private Stack<T> _embeddedScreens;

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

        public void PushEmbeddedScreen(T screen)
        {
            _embeddedScreens.Push(screen);
        }

        public void CloseTopScreen()
        {
            if (_embeddedScreens.TryPop(out T screen))
            {
                screen.OnClose();
            }
        }

        public T TopScreen()
        {
            return _embeddedScreens.Peek();
        }

        public void CloseAllScreens()
        {
            while (_embeddedScreens.TryPop(out T screen))
            {
                screen.OnClose();
            }
        }
    }
}