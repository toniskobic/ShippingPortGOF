namespace ShippingPortGOF.ChainOfResponsibility
{
    public abstract class BaseHandler : IHandler
    {
        protected IHandler? NextHandler { get; set; }

        public BaseHandler()
        {
            NextHandler = null;
        }

        public abstract void Handle(int idShip);

        public void SetNext(IHandler handler)
        {
            NextHandler = handler;
        }
    }
}
