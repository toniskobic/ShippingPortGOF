using ShippingPortGOF.Readers;

namespace ShippingPortGOF.FactoryMethod
{
    public class ShippingPortCreator : Creator
    {
        public override IReader FactoryMethod()
        {
            return new ShippingPortReader();
        }
    }
}
