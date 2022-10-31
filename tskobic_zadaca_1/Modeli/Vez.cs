using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tskobic_zadaca_1.Modeli
{
    public class Vez
    {
        public int ID { get; set; }

        public string OznakaVeza { get; set; }

        public string Vrsta { get; set; }

        public int CijenaPoSatu { get; set; }

        public int MaksDuljina { get; set; }

        public int MaksSirina { get; set; }

        public int MaksDubina { get; set; }

        public Vez(int id, string oznakaVeza, string vrsta,
            int cijenaPoSatu, int maksDuljina, int maksSirina, int maksDubina)
        {
            ID = id;
            OznakaVeza = oznakaVeza;
            Vrsta = vrsta;
            CijenaPoSatu = cijenaPoSatu;
            MaksDuljina = maksDuljina;
            MaksSirina = maksSirina;
            MaksDubina = maksDubina;
        }
    }
}
