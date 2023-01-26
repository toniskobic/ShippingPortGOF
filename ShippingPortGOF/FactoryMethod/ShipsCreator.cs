using ShippingPortGOF.Readers;

namespace ShippingPortGOF.FactoryMethod
{
    public class ShipsCreator : Creator
    {
        public override IReader FactoryMethod()
        {
            return new ShipsReader();
        }
    }
}
