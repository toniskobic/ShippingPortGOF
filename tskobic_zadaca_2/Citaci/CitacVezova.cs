using System.Text.RegularExpressions;
using tskobic_zadaca_2.Modeli;
using tskobic_zadaca_2.Singleton;
using tskobic_zadaca_2.Static;

namespace tskobic_zadaca_2.Citaci
{
    public class CitacVezova : ICitac
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

                    if (celije.Length != 7)
                    {
                        Ispis.GreskaBrojCelija(retci[i], putanja);
                    }
                    else
                    {
                        bool greska = false;

                        for (int j = 0; j < celije.Length; j++)
                        {
                            if (j == 0 || j > 2)
                            {
                                if (!Utils.ProvjeriPretvorbuUInt(celije[j]))
                                {
                                    greska = true;
                                    Ispis.GreskaPretvorbeUInt(retci[i], celije[j], putanja);
                                    break;
                                }
                            }
                            if (j == 2 && !Konstante.VrsteVezova.Contains(celije[j]))
                            {
                                greska = true;
                                Ispis.GreskaNedozvoljenaVrsta(retci[i], celije[j], putanja, "vez");
                                break;
                            }
                        }
                        if (!greska)
                        {
                            int id = int.Parse(celije[0]);
                            int cijenaPoSatu = int.Parse(celije[3]);
                            int maksDuljina = int.Parse(celije[4]);
                            int maksSirina = int.Parse(celije[5]);
                            int maksDubina = int.Parse(celije[6]);

                            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
                            if (!bls.BrodskaLuka!.Vezovi.Exists(x => x.ID == id)
                                && bls.BrodskaLuka.DubinaLuke >= maksDubina && Utils.ProvjeriVezove(celije[2]))
                            {
                                bls.BrodskaLuka.Vezovi.Add(new Vez(id, celije[1], celije[2],
                                    cijenaPoSatu, maksDuljina, maksSirina, maksDubina));
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
