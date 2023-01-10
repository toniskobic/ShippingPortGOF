namespace tskobic_zadaca_3.Visitor
{
    public interface IElement
    {
        public int? Accept(IElementVisitor visitor);
    }
}
