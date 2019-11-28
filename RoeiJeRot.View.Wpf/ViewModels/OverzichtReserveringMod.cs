using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class OverzichtReserveringMod
    {
        public int Id { get; set; }
        public string BoatTypeName { get; set; }
        public int Status { get; set; }
        public bool Reserved { get; set; }
        public DateTime ReservationDate { get; set; }
        public TimeSpan Duration { get; set; }
    }

 
}
