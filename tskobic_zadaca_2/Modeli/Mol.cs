namespace tskobic_zadaca_2.Modeli
{
    public class Mol
    {
        public int ID { get; set; }

        public string Naziv { get; set; }

        public Mol(int id, string naziv)
        {
            ID = id;
            Naziv = naziv;
        }
    }
}
