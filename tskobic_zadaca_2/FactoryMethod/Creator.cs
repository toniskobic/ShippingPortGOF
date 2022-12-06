using tskobic_zadaca_2.Citaci;

namespace tskobic_zadaca_2.FactoryMethod
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
