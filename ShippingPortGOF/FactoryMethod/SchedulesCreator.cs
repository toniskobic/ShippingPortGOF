using ShippingPortGOF.Readers;

namespace ShippingPortGOF.FactoryMethod
{
    public class SchedulesCreator : Creator
    {
        public override IReader FactoryMethod()
        {
            return new SchedulesReader();
        }
    }
}
