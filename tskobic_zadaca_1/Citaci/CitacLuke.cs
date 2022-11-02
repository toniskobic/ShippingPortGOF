using System.Globalization;
using tskobic_zadaca_1.Modeli;
using tskobic_zadaca_1.Singleton;
using tskobic_zadaca_1.Static;

namespace tskobic_zadaca_1.FactoryMethod
{
    public class CitacLuke
    {
        public void ProcitajPodatke(string putanja)
        {
            string[] retci = File.ReadAllLines(putanja);
            string[] celije = retci[1].Split(";");


            if (celije.Length != 8)
            {
                Ispis.GreskaBrojCelija(retci[1], putanja);
            }
            for (int i = 1; i < celije.Length; i++)
            {
                if (i == 1 || i == 2)
                {
                    if (!Validacija.ProvjeriPretvorbuUDouble(celije[i]))
                    {
                        Ispis.GreskaPretvorbeUDouble(retci[1], celije[i], putanja);
                        return;
                    }
                }
                if (i > 2 && i < 7)
                {
                    if (!Validacija.ProvjeriPretvorbuUInt(celije[i]))
                    {
                        Ispis.GreskaPretvorbeUInt(retci[1], celije[i], putanja);
                        return;
                    }
                }
            }

            double gs = double.Parse(celije[1], new CultureInfo("hr-hr"));
            double gd = double.Parse(celije[2], new CultureInfo("hr-hr"));
            int dubinaLuke = int.Parse(celije[3]);
            int putnickiVezovi = int.Parse(celije[4]);
            int poslovniVezovi = int.Parse(celije[5]);
            int ostaliVezovi = int.Parse(celije[6]);

            BrodskaLuka brodskaLuka = new BrodskaLuka(celije[0], gs, gd, dubinaLuke, putnickiVezovi, poslovniVezovi, ostaliVezovi);
            BrodskaLukaSingleton brodskaLukaSingleton = BrodskaLukaSingleton.Instanca();
            brodskaLukaSingleton.BrodskaLuka = brodskaLuka;
            brodskaLukaSingleton.VirtualniSat.VirtualnoVrijeme = DateTime.Parse(celije[7]);
        }
    }
}
