namespace tskobic_zadaca_2.Modeli
{
    public class Kanal
    {
        public int ID { get; set; }

        public int Frekvencija { get; set; }

        public int MaksVeze { get; set; }

        public List<int> Veze { get; set; }

        public Kanal(int id, int frekvencija, int maksVeze)
        {
            ID = id;
            Frekvencija = frekvencija;
            MaksVeze = maksVeze;
            Veze = new List<int>();
        }
    }
}
