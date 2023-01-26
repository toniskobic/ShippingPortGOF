namespace ShippingPortGOF.ChainOfResponsibility
{
    public interface IHandler
    {
        public void SetNext(IHandler handler);

        public void Handle(int idShip);
    }
}
