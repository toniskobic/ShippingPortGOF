namespace tskobic_zadaca_3.Modeli
{
    public class Zapis
    {
        public VrstaZahtjeva Zahtjev { get; set; }

        public int IdBrod { get; set; }

        public bool Odbijen { get; set; }

        public DateTime? VrijemeOd { get; set; }

        public DateTime? VrijemeDo { get; set; }

        public Zapis(VrstaZahtjeva zahtjev, int idBrod, bool odbijen,
            DateTime? vrijemeOd = null, DateTime? vrijemeDo = null)
        {
            Zahtjev = zahtjev;
            IdBrod = idBrod;
            Odbijen = odbijen;
            VrijemeOd = vrijemeOd;
            VrijemeDo = vrijemeDo;
        }
    }
}
