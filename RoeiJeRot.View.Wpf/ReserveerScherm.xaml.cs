using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RoeiJeRot.View.Wpf
{
    /// <summary>
    /// Interaction logic for ReserveerScherm.xaml
    /// </summary>
    public partial class ReserveerScherm : Window
    {
        public ReserveerScherm()
        {
            InitializeComponent();

            When.SelectedDate = DateTime.Today;

            TimeSpan time = new TimeSpan(0, 0, DateTime.Now.Minute, DateTime.Now.Second);
            TimeSpan duration = TimeSpan.FromMinutes(90);
        }
    }
}
