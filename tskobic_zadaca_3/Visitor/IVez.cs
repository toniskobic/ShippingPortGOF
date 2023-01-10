namespace tskobic_zadaca_3.Visitor
{
    public interface IVez
    {
        public string Accept(IVezVisitor visitor);
    }
}
