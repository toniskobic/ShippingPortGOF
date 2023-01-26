using ShippingPortGOF.Readers;

namespace ShippingPortGOF.FactoryMethod
{
    public class ChannelsCreator : Creator
    {
        public override IReader FactoryMethod()
        {
            return new ChannelsReader();
        }
    }
}
