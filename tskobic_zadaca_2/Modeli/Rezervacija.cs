namespace tskobic_zadaca_2.Modeli
{
    public class Rezervacija
    {
        public int IDVez { get; set; }

        public int IDBrod { get; set; }

        public DateTime DatumOd { get; set; }

        public int SatiTrajanja { get; set; }

        public Rezervacija(int idVez, int idBrod, DateTime datumOd, int satiTrajanja)
        {
            IDVez = idVez;
            IDBrod = idBrod;
            DatumOd = datumOd;
            SatiTrajanja = satiTrajanja;
        }
    }
}
