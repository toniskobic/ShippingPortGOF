using tskobic_zadaca_3.Singleton;

namespace tskobic_zadaca_3.Observer
{
    public class Brod : IObserver
    {
        #region Svojstva
        public int ID { get; set; }

        public string OznakaBroda { get; set; }

        public string Naziv { get; set; }

        public string Vrsta { get; set; }

        public double Duljina { get; set; }

        public double Sirina { get; set; }

        public double Gaz { get; set; }

        public double MaksBrzina { get; set; }

        public int KapacitetPutnika { get; set; }

        public int KapacitetOsobnihVozila { get; set; }

        public int KapacitetTereta { get; set; }

        public string? ZadnjaPoruka { get; set; }
        #endregion

        #region Konstruktor
        public Brod(int id, string oznakaBroda, string naziv, string vrsta,
            double duljina, double sirina, double gaz, double maksBrzina,
            int kapacitetPutnika, int kapacitetOsobnihVozila, int kapacitetTereta)
        {
            ID = id;
            OznakaBroda = oznakaBroda;
            Naziv = naziv;
            Vrsta = vrsta;
            Duljina = duljina;
            Sirina = sirina;
            Gaz = gaz;
            MaksBrzina = maksBrzina;
            KapacitetPutnika = kapacitetPutnika;
            KapacitetOsobnihVozila = kapacitetOsobnihVozila;
            KapacitetTereta = kapacitetTereta;
        }
        #endregion

        #region Metode
        public void Update(ISubject s)
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            ZadnjaPoruka = s.GetState();
            bls.Controller.SetModelState($"Brod {ID} zaprimio poruku: '{ZadnjaPoruka}'.");
        }
        #endregion
    }
}
