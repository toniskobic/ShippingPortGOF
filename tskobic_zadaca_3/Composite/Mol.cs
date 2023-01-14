namespace tskobic_zadaca_3.Composite
{
    public class Mol : IComponent
    {
        public int ID { get; set; }

        public string Naziv { get; set; }

        public Mol(int id, string naziv)
        {
            ID = id;
            Naziv = naziv;
        }

        public int GetId()
        {
            return ID;
        }

        public string GetName()
        {
            return Naziv;
        }

        public bool IsComposite()
        {
            return false;
        }
    }
}
