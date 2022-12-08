namespace tskobic_zadaca_2.ChainOfResponsibility
{
    public interface IHandler
    {
        public void SetNext(IHandler handler);

        public void Handle(int idBrod);
    }
}
