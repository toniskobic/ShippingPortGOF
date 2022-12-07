namespace tskobic_zadaca_2.Visitor
{
    public interface IElement
    {
        public int? Accept(IElementVisitor visitor);
    }
}
