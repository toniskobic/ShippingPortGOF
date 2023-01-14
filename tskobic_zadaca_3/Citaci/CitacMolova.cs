using System.Text.RegularExpressions;
using tskobic_zadaca_3.Composite;
using tskobic_zadaca_3.Singleton;
using tskobic_zadaca_3.Static;

namespace tskobic_zadaca_3.Citaci
{
    public class CitacMolova : ICitac
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

                    if (celije.Length != 2)
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

                        BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
                        List<IComponent> molovi = bls.BrodskaLuka!.Find(c => c is Mol);
                        if (!molovi.Exists(x => x.GetId()== id))
                        {
                            bls.BrodskaLuka.Add(new Mol(id, celije[1]));
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
