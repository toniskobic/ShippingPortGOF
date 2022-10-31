using System.Globalization;
using tskobic_zadaca_1.Modeli;
using tskobic_zadaca_1.Singleton;

namespace tskobic_zadaca_1.Citaci
{
    public class CitacBrodova
    {
        private readonly string[] vrsteBrodova = { "TR", "KA", "KL", "KR", "RI", "TE", "JA", "BR", "RO" };

        public List<Brod> ProcitajPodatke(string putanja)
        {
            string[] retci = File.ReadAllLines(putanja);
            List<Brod> brodovi = new List<Brod>();

            for (int i = 1; i < retci.Length; i++)
            {
                string[] celije = retci[i].Split(";");


                if (celije.Length != 11)
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
                        if (j == 0 || j > 7)
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
                        if (j == 3 && !vrsteBrodova.Contains(celije[j]))
                        {
                            greska = true;
                            Console.WriteLine($"ERROR: Neispravan redak {retci[i]} u datoteci {putanja}."
                                + $" Ćelija {celije[j]} ima nedozvoljenu vrstu broda. "
                                + $"Broj greške: {++BrodskaLukaSingleton.Instanca().BrojGreski}");
                            break;
                        }
                        if (j > 3 && j < 8)
                        {
                            if (!double.TryParse(celije[j], NumberStyles.Float, new CultureInfo("hr-hr"), out double rezultat))
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
                        double duljina = double.Parse(celije[4], new CultureInfo("hr-hr"));
                        double sirina = double.Parse(celije[5], new CultureInfo("hr-hr"));
                        double gaz = double.Parse(celije[6], new CultureInfo("hr-hr"));
                        double maksBrzina = double.Parse(celije[7], new CultureInfo("hr-hr"));
                        int kapacitetPutnika = int.Parse(celije[8]);
                        int kapacitetOsobnihVozila = int.Parse(celije[9]);
                        int kapacitetTereta = int.Parse(celije[10]);

                        Brod brod = new Brod(id, celije[1], celije[2], celije[3],
                            duljina, sirina, gaz, maksBrzina, kapacitetPutnika,
                            kapacitetOsobnihVozila, kapacitetTereta);
                        brodovi.Add(brod);
                    }
                }
            }

            return brodovi;
        }

    }
}
