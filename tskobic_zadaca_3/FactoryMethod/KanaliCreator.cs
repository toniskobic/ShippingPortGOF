using tskobic_zadaca_3.Citaci;

namespace tskobic_zadaca_3.FactoryMethod
{
    public class KanaliCreator : Creator
    {
        public override ICitac FactoryMethod()
        {
            return new CitacKanala();
        }
    }
}
