using RoeiJeRot.View.Wpf;
using RoeiJeRot.View.Wpf.Models;
using RoeiJeRot.Database.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoeiJeRot.Logic.Services;


namespace RoeiJeRot.View.Wpf
{
    public class OverzichtReserveringModel
    {

        public ObservableCollection<SailingBoat> Boten { get; set; }

        public OverzichtReserveringModel(IBoatService boatService)
        {
            Boten = new ObservableCollection<SailingBoat>(boatService.GetBoats());
        }
    }
}
