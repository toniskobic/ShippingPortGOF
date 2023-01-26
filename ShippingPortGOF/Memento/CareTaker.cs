namespace ShippingPortGOF.Memento
{
    public class CareTaker
    {
        private List<Memento> MementoList { get; set; }

        public CareTaker()
        {
            MementoList = new List<Memento>();
        }

        public void Add (Memento state)
        {
            MementoList.Add(state);
        }

        public Memento? Get(string name)
        {
            return MementoList.FirstOrDefault(m => m.Name == name);
        }
    }
}
