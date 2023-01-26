namespace ShippingPortGOF.Visitor
{
    public interface IElement
    {
        public int? Accept(IElementVisitor visitor);
    }
}
