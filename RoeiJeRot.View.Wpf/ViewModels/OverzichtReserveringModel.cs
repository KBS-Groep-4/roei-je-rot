using RoeiJeRot.View.Wpf;
using RoeiJeRot.View.Wpf.Models;
using RoeiJeRot.Database.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoeiJeRot.View.Wpf
{
    public class OverzichtReserveringModel
    {
        public ObservableCollection<SailingReservation> Boten { get; set; }

        public OverzichtReserveringModel()
        {
            DataAccess da = new DataAccess();
            Boten = new ObservableCollection<SailingReservation>();
        }
    }
}
