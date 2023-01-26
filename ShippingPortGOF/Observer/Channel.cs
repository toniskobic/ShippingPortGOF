namespace ShippingPortGOF.Observer
{
    public class Channel : ISubject
    {
        public int ID { get; set; }

        public int Frequency { get; set; }

        public int MaxConnections { get; set; }

        public string? LastMessage { get; set; }

        public List<IObserver> Observers { get; set; }

        public Channel(int id, int frequency, int maxMoorings)
        {
            ID = id;
            Frequency = frequency;
            MaxConnections = maxMoorings;
            Observers = new List<IObserver>();
        }

        public bool IsBusy()
        {
            return Observers.Count >= MaxConnections;
        }

        public void Attach(IObserver observer)
        {
            Observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            Observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (IObserver observer in Observers)
            {
                observer.Update(this);
            }
        }

        public string? GetState()
        {
            return LastMessage;
        }

        public void SetState(string? state)
        {
            LastMessage = state;
            Notify();
        }
    }
}
