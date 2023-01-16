namespace tskobic_zadaca_3.Observer
{
    public class Kanal : ISubject
    {
        public int ID { get; set; }

        public int Frekvencija { get; set; }

        public int MaksVeze { get; set; }

        public string? ZadnjaPoruka { get; set; }

        public List<IObserver> Observers { get; set; }

        public Kanal(int id, int frekvencija, int maksVeze)
        {
            ID = id;
            Frekvencija = frekvencija;
            MaksVeze = maksVeze;
            Observers = new List<IObserver>();
        }

        public bool Zauzet()
        {
            return Observers.Count >= MaksVeze;
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
            return ZadnjaPoruka;
        }

        public void SetState(string? state)
        {
            ZadnjaPoruka = state;
            Notify();
        }
    }
}
