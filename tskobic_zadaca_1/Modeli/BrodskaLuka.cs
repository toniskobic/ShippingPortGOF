using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tskobic_zadaca_1.Modeli
{
    public class BrodskaLuka
    {
        public string Naziv { get; set; }

        public double GPSSirina { get; set; }

        public double GPSVisina { get; set; }

        public int DubinaLuke { get; set; }

        public int PutnickiVezovi { get; set; }

        public int PoslovniVezovi { get; set; }

        public int OstaliVezovi { get; set; }

        public List<Vez> Vezovi { get; set; }

        public List<Brod> Brodovi { get; set; }

        public BrodskaLuka(string naziv, double gpsSirina, double gpsVisina, int dubinaLuke,
            int putnickiVezovi, int poslovniVezovi, int ostaliVezovi)
        {
            Naziv = naziv;
            GPSSirina = gpsSirina;
            GPSVisina = gpsVisina;
            DubinaLuke = dubinaLuke;
            PutnickiVezovi = putnickiVezovi;
            PoslovniVezovi = poslovniVezovi;
            OstaliVezovi = ostaliVezovi;
            Vezovi = new List<Vez>();
            Brodovi = new List<Brod>();
        }
    }
}
