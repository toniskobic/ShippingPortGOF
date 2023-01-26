namespace ShippingPortGOF.Visitor
{
    public interface IMooring
    {
        public string Accept(IMooringVisitor visitor);
    }
}
