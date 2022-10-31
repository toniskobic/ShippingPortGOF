using System.Globalization;
using tskobic_zadaca_1.Modeli;
using tskobic_zadaca_1.Singleton;

namespace tskobic_zadaca_1.FactoryMethod
{
    public class CitacLuke
    {
        public void ProcitajPodatke(string putanja)
        {
            string[] retci = File.ReadAllLines(putanja);
            string[] celije = retci[1].Split(";");

            for (int i = 1; i < celije.Length; i++)
            {
                if (i == 1 || i == 2)
                {
                    if (!double.TryParse(celije[i], NumberStyles.Float, new CultureInfo("hr-hr"), out double rezultat))
                    {
                        Console.WriteLine($"ERROR: Neispravan redak {retci[1]} u datoteci {putanja}."
                            + $" Ćeliju {celije[i]} nije moguće pretvoriti u broj. "
                            + $"Broj greške: {++BrodskaLukaSingleton.Instanca().BrojGreski}");

                        return;
                    }
                }
                if (i > 2 && i < 7)
                {
                    if (!int.TryParse(celije[i], out int rezultat))
                    {
                        Console.WriteLine($"ERROR: Neispravan redak {retci[1]} u datoteci {putanja}."
                            + $" Ćeliju {celije[i]} nije moguće pretvoriti u broj. "
                            + $"Broj greške: {++BrodskaLukaSingleton.Instanca().BrojGreski}");

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
            brodskaLukaSingleton.VirtualnoVrijeme = DateTime.Parse(celije[7]);
        }

    }
}
