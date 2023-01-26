using ShippingPortGOF.Models;

namespace ShippingPortGOF.Visitor
{
    public class MooringVisitor : IMooringVisitor
    {
        public string Visit(Mooring mooring)
        {
            return mooring.Type;
        }
    }
}
