namespace ShippingPortGOF.Composite
{
    public class Pier : IComponent
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public Pier(int id, string name)
        {
            ID = id;
            Name = name;
        }

        public int GetId()
        {
            return ID;
        }

        public string GetName()
        {
            return Name;
        }

        public bool IsComposite()
        {
            return false;
        }
    }
}
