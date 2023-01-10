namespace tskobic_zadaca_3.ChainOfResponsibility
{
    public abstract class BaseHandler : IHandler
    {
        protected IHandler? NextHandler { get; set; }

        public BaseHandler()
        {
            NextHandler = null;
        }

        public abstract void Handle(int idBrod);

        public void SetNext(IHandler handler)
        {
            NextHandler = handler;
        }
    }
}
