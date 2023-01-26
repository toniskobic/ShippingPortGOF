using ShippingPortGOF.Readers;

namespace ShippingPortGOF.FactoryMethod
{
    public class PiersCreator : Creator
    {
        public override IReader FactoryMethod()
        {
            return new PiersReader();
        }

    }
}
