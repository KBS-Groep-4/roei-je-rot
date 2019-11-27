using RoeiJeRot.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for OverzichtReservering.xaml
    /// </summary>
    public partial class OverzichtReserveringen : Window
    {
        public OverzichtReserveringen(IBoatService boatService)
        {
            InitializeComponent();
            Boten.ItemsSource = boatService.GetBoats();
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Boten_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Boden_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
