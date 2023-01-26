using ShippingPortGOF.Models;

namespace ShippingPortGOF.Visitor
{
    public interface IMooringVisitor
    {
        public string Visit(Mooring mooring);
    }
}
