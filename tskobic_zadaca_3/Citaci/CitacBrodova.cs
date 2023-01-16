using System.Globalization;
using System.Text.RegularExpressions;
using tskobic_zadaca_3.Composite;
using tskobic_zadaca_3.Observer;
using tskobic_zadaca_3.Singleton;
using tskobic_zadaca_3.Static;

namespace tskobic_zadaca_3.Citaci
{
    public class CitacBrodova : ICitac
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

                for (int i = 1; i < retci.Length; i++)
                {
                    string[] celije = retci[i].Split(";");

                    if (celije.Length != 11)
                    {
                        Ispis.GreskaBrojCelija(retci[i], putanja);
                    }
                    else
                    {
                        bool greska = false;

                        for (int j = 0; j < celije.Length; j++)
                        {
                            if (j == 0 || j > 7)
                            {
                                if (!Utils.ProvjeriPretvorbuUInt(celije[j]))
                                {
                                    greska = true;
                                    Ispis.GreskaPretvorbeUInt(retci[i], celije[j], putanja);
                                    break;
                                }
                            }
                            if (j == 3 && !Konstante.VrsteBrodova.Contains(celije[j]))
                            {
                                greska = true;
                                Ispis.GreskaNedozvoljenaVrsta(retci[i], celije[j], putanja, "brod");
                                break;
                            }
                            if (j > 3 && j < 8 && !Utils.ProvjeriPretvorbuUDouble(celije[j]))
                            {
                                greska = true;
                                Ispis.GreskaPretvorbeUDouble(retci[i], celije[j], putanja);
                                break;
                            }
                        }
                        if (!greska)
                        {
                            int id = int.Parse(celije[0]);
                            double duljina = double.Parse(celije[4], new CultureInfo("hr-hr"));
                            double sirina = double.Parse(celije[5], new CultureInfo("hr-hr"));
                            double gaz = double.Parse(celije[6], new CultureInfo("hr-hr"));
                            double maksBrzina = double.Parse(celije[7], new CultureInfo("hr-hr"));
                            int kapacitetPutnika = int.Parse(celije[8]);
                            int kapacitetOsobnihVozila = int.Parse(celije[9]);
                            int kapacitetTereta = int.Parse(celije[10]);

                            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
                            if (!bls.BrodskaLuka!.Brodovi.Exists(x => x.ID == id) && bls.BrodskaLuka.DubinaLuke > gaz)
                            {
                                Brod brod = new Brod(id, celije[1], celije[2], celije[3],
                                    duljina, sirina, gaz, maksBrzina, kapacitetPutnika,
                                    kapacitetOsobnihVozila, kapacitetTereta);
                                bls.BrodskaLuka.Brodovi.Add(brod);
                            }
                        }
                    }
                }
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
