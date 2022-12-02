namespace tskobic_zadaca_2.Modeli
{
    public class Rezervacija
    {
        public int IDVez { get; set; }

        public int IDBrod { get; set; }

        public DateTime DatumOd { get; set; }

        public int SatiTrajanja { get; set; }

        public Rezervacija(int iDVez, int iDBrod, DateTime datumOd, int satiTrajanja)
        {
            IDVez = iDVez;
            IDBrod = iDBrod;
            DatumOd = datumOd;
            SatiTrajanja = satiTrajanja;
        }
    }
}
