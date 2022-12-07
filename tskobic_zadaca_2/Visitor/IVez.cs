namespace tskobic_zadaca_2.Visitor
{
    public interface IVez
    {
        public string Accept(IVezVisitor visitor);
    }
}
