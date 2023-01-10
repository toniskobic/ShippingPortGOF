namespace tskobic_zadaca_3.ChainOfResponsibility
{
    public interface IHandler
    {
        public void SetNext(IHandler handler);

        public void Handle(int idBrod);
    }
}
