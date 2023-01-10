using tskobic_zadaca_3.Citaci;

namespace tskobic_zadaca_3.FactoryMethod
{
    public class RezervacijeCreator : Creator
    {
        public override ICitac FactoryMethod()
        {
            return new CitacRezervacija();
        }
    }
}
