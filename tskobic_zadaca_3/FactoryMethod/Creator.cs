using tskobic_zadaca_3.Citaci;

namespace tskobic_zadaca_3.FactoryMethod
{
    public abstract class Creator
    {
        private ICitac? citac;

        public abstract ICitac FactoryMethod();

        public void ProcitajPodatke(string putanja)
        {
            citac = FactoryMethod();
            citac.ProcitajPodatke(putanja);
        }
    }
}
