namespace tskobic_zadaca_1.Modeli
{
    public class Raspored
    {
        public int IDVez { get; set; }

        public int IDBrod { get; set; }

        public List<DayOfWeek> DaniUTjednu { get; set; }

        public TimeOnly VrijemeOd { get; set; }

        public TimeOnly VrijemeDo { get; set; }

        public Raspored(int idVez, int idBrod, List<DayOfWeek> daniUTjednu, TimeOnly vrijemeOd, TimeOnly vrijemeDo)
        {
            IDVez = idVez;
            IDBrod = idBrod;
            DaniUTjednu = daniUTjednu;
            VrijemeOd = vrijemeOd;
            VrijemeDo = vrijemeDo;
        }
    }
}
