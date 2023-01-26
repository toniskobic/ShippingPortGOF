namespace ShippingPortGOF.Observer
{
    public interface ISubject
    {
        public void Attach(IObserver observer);

        public void Detach(IObserver observer);

        public void Notify();

        public string? GetState();
        
        public void SetState(string? state);
    }
}
