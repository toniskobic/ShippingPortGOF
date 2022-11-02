using System.Globalization;
using tskobic_zadaca_1.Modeli;
using tskobic_zadaca_1.Singleton;

namespace tskobic_zadaca_1.Static
{
    public static class Validacija
    {
        public static bool ProvjeriPretvorbuUDouble(string celija)
        {
            return double.TryParse(celija, NumberStyles.Float, new CultureInfo("hr-hr"), out double rezultat);
        }

        public static bool ProvjeriPretvorbuUInt(string celija)
        {
            return int.TryParse(celija, out int rezultat);
        }

        public static bool ProvjeriVezove(string vrsta)
        {
            BrodskaLukaSingleton brodskaLukaSingleton = BrodskaLukaSingleton.Instanca();
            BrodskaLuka brodskaLuka = brodskaLukaSingleton.BrodskaLuka;

            bool provjera = false;

            switch (vrsta)
            {
                case "PU":
                    {
                        provjera = brodskaLuka.PutnickiVezovi > brodskaLuka.Vezovi.FindAll(x => x.Vrsta == "PU").Count;
                        break;
                    }
                case "PO":
                    {
                        provjera = brodskaLuka.PoslovniVezovi > brodskaLuka.Vezovi.FindAll(x => x.Vrsta == "PO").Count;
                        break;
                    }
                case "OS":
                    {
                        provjera = brodskaLuka.OstaliVezovi > brodskaLuka.Vezovi.FindAll(x => x.Vrsta == "OS").Count;
                        break;
                    }
            }

            return provjera;
        }
    }
}
