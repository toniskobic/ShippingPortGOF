using tskobic_zadaca_2.Citaci;

namespace tskobic_zadaca_2.FactoryMethod
{
    public class BrodoviCreator : Creator
    {
        public override ICitac FactoryMethod()
        {
            return new CitacBrodova();
        }
    }
}
