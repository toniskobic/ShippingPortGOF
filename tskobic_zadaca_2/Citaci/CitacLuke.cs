using System.Globalization;
using System.Text.RegularExpressions;
using tskobic_zadaca_2.Modeli;
using tskobic_zadaca_2.Singleton;
using tskobic_zadaca_2.Static;

namespace tskobic_zadaca_2.Citaci
{
    public class CitacLuke : ICitac
    {
        public void ProcitajPodatke(string putanja)
        {
            try
            {
                if (!ProvjeriInformativniRedak(putanja))
                {
                    return;
                }
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
                        if (!Utils.ProvjeriPretvorbuUDouble(celije[i]))
                        {
                            Ispis.GreskaPretvorbeUDouble(retci[1], celije[i], putanja);
                            return;
                        }
                    }
                    if (i > 2 && i < 7 && !Utils.ProvjeriPretvorbuUInt(celije[i]))
                    {
                        Ispis.GreskaPretvorbeUInt(retci[1], celije[i], putanja);
                        return;
                    }
                }
                if (!Utils.ProvjeriPretvorbuUDatum(celije[7], out DateTime virtualnoVrijeme))
                {
                    Ispis.GreskaPretvorbeUDatum(retci[1], celije[7], putanja);
                    return;
                }

                double gs = double.Parse(celije[1], new CultureInfo("hr-hr"));
                double gd = double.Parse(celije[2], new CultureInfo("hr-hr"));
                int dubinaLuke = int.Parse(celije[3]);
                int putnickiVezovi = int.Parse(celije[4]);
                int poslovniVezovi = int.Parse(celije[5]);
                int ostaliVezovi = int.Parse(celije[6]);

                BrodskaLuka brodskaLuka = new BrodskaLuka(celije[0], gs, gd, dubinaLuke, putnickiVezovi, poslovniVezovi, ostaliVezovi);
                BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
                bls.BrodskaLuka = brodskaLuka;
                bls.VirtualniSat.VirtualnoVrijeme = virtualnoVrijeme;
            }
            catch (Exception)
            {
                return;
            }
        }

        public bool ProvjeriInformativniRedak(string putanja)
        {
            Regex rg = new Regex(Konstante.InformativniRedak);
            string redak = File.ReadLines(putanja).First();
            if (!rg.IsMatch(redak))
            {
                Ispis.GreskaNeispravanInformativniRedak(redak, putanja);
                return false;
            }

            return true;
        }
    }
}
