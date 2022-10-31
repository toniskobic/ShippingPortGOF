using tskobic_zadaca_1.Modeli;
using tskobic_zadaca_1.Singleton;

namespace tskobic_zadaca_1.Citaci
{
    public class CitacVezova
    {
        public List<Vez> ProcitajPodatke(string putanja)
        {
            string[] retci = File.ReadAllLines(putanja);
            List<Vez> vezovi = new List<Vez>();

            for (int i = 1; i < retci.Length; i++)
            {
                string[] celije = retci[i].Split(";");


                if (celije.Length != 7)
                {
                    Console.WriteLine($"ERROR: Neispravan redak {retci[i]} u datoteci {putanja}."
                        + $" Broj ćelija je neispravan. "
                        + $"Broj greške: {++BrodskaLukaSingleton.Instanca().BrojGreski}");
                }
                else
                {
                    bool greska = false;

                    for (int j = 0; j < celije.Length; j++)
                    {
                        if (j == 0 || j > 2)
                        {
                            if (!int.TryParse(celije[j], out int rezultat))
                            {
                                greska = true;
                                Console.WriteLine($"ERROR: Neispravan redak {retci[i]} u datoteci {putanja}."
                                    + $" Ćeliju {celije[j]} nije moguće pretvoriti u broj. "
                                    + $"Broj greške: {++BrodskaLukaSingleton.Instanca().BrojGreski}");
                                break;
                            }
                        }
                    }
                    if (!greska)
                    {
                        int id = int.Parse(celije[0]);
                        int cijenaPoSatu = int.Parse(celije[3]);
                        int maksDuljina = int.Parse(celije[4]);
                        int maksSirina = int.Parse(celije[5]);
                        int maksDubina = int.Parse(celije[6]);

                        BrodskaLukaSingleton brodskaLukaSingleton = BrodskaLukaSingleton.Instanca();
                        BrodskaLuka brodskaLuka = brodskaLukaSingleton.BrodskaLuka;
                        switch (celije[2])
                        {
                            case "PU":
                                {
                                    if (brodskaLuka.PutnickiVezovi > brodskaLuka.Vezovi.FindAll(x => x.Vrsta == "PU").Count)
                                    {
                                        vezovi.Add(new Vez(id, celije[1], celije[2],
                                            cijenaPoSatu, maksDuljina, maksSirina, maksDubina));
                                    }
                                    break;
                                }
                            case "PO":
                                {
                                    if (brodskaLuka.PoslovniVezovi > brodskaLuka.Vezovi.FindAll(x => x.Vrsta == "PO").Count)
                                    {
                                        vezovi.Add(new Vez(id, celije[1], celije[2],
                                            cijenaPoSatu, maksDuljina, maksSirina, maksDubina));
                                    }
                                    break;
                                }
                            case "OS":
                                {
                                    if (brodskaLuka.OstaliVezovi > brodskaLuka.Vezovi.FindAll(x => x.Vrsta == "OS").Count)
                                    {
                                        vezovi.Add(new Vez(id, celije[1], celije[2],
                                            cijenaPoSatu, maksDuljina, maksSirina, maksDubina));
                                    }
                                    break;
                                }
                            default:
                                {
                                    Console.WriteLine($"ERROR: Neispravna redak {retci[i]} u datoteci {putanja}."
                                    + $" Ćelija {celije[2]} ima nedozvoljenu vrstu veza."
                                    + $"Broj greške: {++BrodskaLukaSingleton.Instanca().BrojGreski}");
                                }
                                break;
                        }
                    }
                }
            }

            return vezovi;
        }
    }
}
