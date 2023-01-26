namespace ShippingPortGOF.Memento
{
    public class VirtualTimeOriginator
    {
        public DateTime VirtualTime { get; set; }

        public DateTime RealTime { get; set; }

        public void ShiftVirtualTime()
        {
            DateTime currentTime = DateTime.Now;
            TimeSpan timePassed = currentTime.Subtract(RealTime);
            RealTime = currentTime;
            VirtualTime = VirtualTime.Add(timePassed);
        }

        public void GetStateFromMemento(Memento m)
        {
            VirtualTime = m.VirtualTime;
        }

        public Memento SaveStateToMemento(string name)
        {
            return new Memento(name, VirtualTime);
        }
    }
}
