using ShippingPortGOF.Readers;

namespace ShippingPortGOF.FactoryMethod
{
    public class MooringsPiersCreator : Creator
    {
        public override IReader FactoryMethod()
        {
            return new PiersMooringsReader();
        }
    }
}
