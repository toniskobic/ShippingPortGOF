using ShippingPortGOF.Observer;

namespace ShippingPortGOF.MVC
{
    public class Model : ISubject
    {
        private string? State { get; set; }

        public List<IObserver> Observers { get; set; }

        public Model()
        {
            Observers = new List<IObserver>();
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
            return State;
        }

        public void SetState(string? state)
        {
            State = state;
            Notify();
        }
    }
}
