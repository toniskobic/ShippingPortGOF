namespace tskobic_zadaca_2.Modeli
{
    public class Privez
    {
        public int IDVez { get; set; }

        public int IDBrod { get; set; }

        public DateTime VrijemeOd { get; set; }

        public DateTime VrijemeDo { get; set; }

        public Privez(int iDVez, int iDBrod, DateTime vrijemeOd, DateTime vrijemeDo)
        {
            IDVez = iDVez;
            IDBrod = iDBrod;
            VrijemeOd = vrijemeOd;
            VrijemeDo = vrijemeDo;
        }
    }
}
