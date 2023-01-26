namespace ShippingPortGOF.Memento
{
    public class Memento
    {
        public string Name { get; set; }

        public DateTime VirtualTime { get; set; }

        public Memento(string name, DateTime virtualTime)
        {
            Name = name;
            VirtualTime = virtualTime;
        }
    }
}
