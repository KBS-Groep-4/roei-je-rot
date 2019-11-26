using RoeiJeRot.View.Wpf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoeiJeRot.View.Wpf
{
    public class DataAccess
    {
        Random rnd = new Random();
        string[] boten = new string[] { "Boot", "Boot1" };
        string[] aantallen = new string[] { "2", "3" };
        bool[] state = new bool[] { true, false };

        public List<BotenModel> GetBoten(int total = 10)
        {
            List<BotenModel> output = new List<BotenModel>();

            for (int i = 0; i < total; i++)
            {

                output.Add(GetBoot(i + 1));
            }

            return output;
        }

        private BotenModel GetBoot(int id)
        {
            BotenModel output = new BotenModel();

            output.ID = id;
            output.Boot = GetRandomItem(boten);
            output.Aantal = GetRandomItem(aantallen);
            output.Status = GetRandomItem(state);


            return output;
        }
        private T GetRandomItem<T>(T[] data)
        {
            return data[rnd.Next(0, data.Length)];
        }
    }
}

