namespace tskobic_zadaca_1.Modeli
{
    public class Brod
    {
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
    }
}
