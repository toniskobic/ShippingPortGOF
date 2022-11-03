using tskobic_zadaca_1.Citaci;

namespace tskobic_zadaca_1.FactoryMethod
{
    public class RasporediCreator : Creator
    {
        public override ICitac FactoryMethod()
        {
            return new CitacRasporeda();
        }
    }
}
