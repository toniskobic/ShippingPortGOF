using ShippingPortGOF.Readers;

namespace ShippingPortGOF.FactoryMethod
{
    public class ReservationsCreator : Creator
    {
        public override IReader FactoryMethod()
        {
            return new ReservationsReader();
        }
    }
}
