using System.Text.RegularExpressions;
using tskobic_zadaca_3.Modeli;
using tskobic_zadaca_3.Singleton;
using tskobic_zadaca_3.Static;

namespace tskobic_zadaca_3.Citaci
{
    internal class CitacKanala : ICitac
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

                    if (celije.Length != 3)
                    {
                        Ispis.GreskaBrojCelija(retci[i], putanja);
                    }
                    else
                    {
                        if (!int.TryParse(celije[0], out int id))
                        {
                            Ispis.GreskaPretvorbeUInt(retci[i], celije[0], putanja);
                            continue;
                        }
                        if (!int.TryParse(celije[1], out int frekvencija))
                        {
                            Ispis.GreskaPretvorbeUInt(retci[i], celije[1], putanja);
                            continue;
                        }
                        if (!int.TryParse(celije[2], out int maksVeze))
                        {
                            Ispis.GreskaPretvorbeUInt(retci[i], celije[2], putanja);
                            continue;
                        }

                        BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
                        if (!bls.BrodskaLuka!.Kanali.Exists(x => x.ID == id || x.Frekvencija == frekvencija))
                        {
                            bls.BrodskaLuka.Kanali.Add(new Kanal(id, frekvencija, maksVeze));
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
