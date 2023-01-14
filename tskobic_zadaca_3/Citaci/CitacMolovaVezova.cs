using System.Text.RegularExpressions;
using tskobic_zadaca_3.Composite;
using tskobic_zadaca_3.Modeli;
using tskobic_zadaca_3.Singleton;
using tskobic_zadaca_3.Static;

namespace tskobic_zadaca_3.Citaci
{
    public class CitacMolovaVezova : ICitac
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
                        if (!int.TryParse(celije[0], out int idMol))
                        {
                            Ispis.GreskaPretvorbeUInt(retci[i], celije[0], putanja);
                            continue;
                        }
                        if (!ProvjeriVezove(celije[1]))
                        {
                            Ispis.GreskaPretvorbeVezova(retci[i], celije[1], putanja);
                            continue;
                        }
                        string[] zapisi = celije[1].Split(",");
                        List<int> vezovi = zapisi.Select(int.Parse).ToList();

                        BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
                        List<IComponent> molovi = bls.BrodskaLuka!.Find(c => c is Mol);
                        if (!molovi.Exists(x => x.GetId() == idMol))
                        {
                            Ispis.GreskaNepostojeciZapis(retci[i], celije[0], putanja, idMol, "Mol");
                            continue;
                        }
                        foreach (int vez in vezovi)
                        {
                            List<IComponent> postojeciVezovi = bls.BrodskaLuka!.Find(c => c is Vez);
                            Vez? postojeciVez = (Vez?)postojeciVezovi.Find(x => x.GetId() == vez);
                            if(postojeciVez == null)
                            {
                                Ispis.GreskaNepostojeciZapis(retci[i], celije[1], putanja, vez, "Vez");
                                continue;
                            }
                            if(postojeciVez.IdMol == null)
                            {
                                postojeciVez.IdMol = idMol;
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

        public bool ProvjeriVezove(string vezovi)
        {
            Regex rg = new Regex(Konstante.Vezovi);
            if (!rg.IsMatch(vezovi))
            {
                return false;
            }

            return true;
        }
    }
}
