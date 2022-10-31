using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tskobic_zadaca_1.Modeli
{
    public class Raspored
    {
        public int IDVez { get; set; }

        public int IDBrod { get; set; }

        public int[] DaniUTjednu { get; set; } = new int[7];

        public DateTime VrijemeOd { get; set; }

        public DateTime VrijemeDo { get; set; }

        public Raspored(int idVez, int idBrod, int[] daniUTjednu, DateTime vrijemeOd, DateTime vrijemeDo)
        {
            IDVez = idVez;
            IDBrod = idBrod;
            DaniUTjednu = daniUTjednu;
            VrijemeOd = vrijemeOd;
            VrijemeDo = vrijemeDo;
        }
    }
}
