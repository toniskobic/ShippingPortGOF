using ShippingPortGOF.Readers;

namespace ShippingPortGOF.FactoryMethod
{
    public class MooringsCreator : Creator
    {
        public override IReader FactoryMethod()
        {
            return new MooringsReader();
        }
    }
}
