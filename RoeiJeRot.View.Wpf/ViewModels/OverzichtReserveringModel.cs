using RoeiJeRot.View.Wpf;
using RoeiJeRot.View.Wpf.Models;
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
        public ObservableCollection<BotenModel> _Boten { get; set; }

        public OverzichtReserveringModel()
        {
            DataAccess da = new DataAccess();
            _Boten = new ObservableCollection<BotenModel>(da.GetBoten());
        }
    }
}
