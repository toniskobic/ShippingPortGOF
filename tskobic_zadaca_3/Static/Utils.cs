using System.Globalization;
using tskobic_zadaca_3.Modeli;
using tskobic_zadaca_3.Singleton;

namespace tskobic_zadaca_3.Static
{
    public static class Utils
    {
        public static bool ProvjeriPretvorbuUDouble(string celija)
        {
            return double.TryParse(celija, NumberStyles.Float, new CultureInfo("hr-hr"), out _);
        }

        public static bool ProvjeriPretvorbuUInt(string celija)
        {
            return int.TryParse(celija, out _);
        }

        public static bool ProvjeriPretvorbuUDatum(string celija, out DateTime datumOd)
        {
            return DateTime.TryParseExact(celija, "dd.MM.yyyy. HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out datumOd);
        }

        public static List<Vez> PronadjiVezove(Brod brod)
        {
            string? vez = null;
            switch (brod.Vrsta)
            {
                case string ulaz when Konstante.PutnickiBrodovi.Contains(ulaz):
                    {
                        vez = "PU";
                        break;
                    }
                case string ulaz when Konstante.PoslovniBrodovi.Contains(ulaz):
                    {
                        vez = "PO";
                        break;
                    }
                case string ulaz when Konstante.OstaliBrodovi.Contains(ulaz):
                    {
                        vez = "OS";
                        break;
                    }
            }
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            BrodskaLuka? brodskaLuka = bls.BrodskaLuka;
            List<Vez> vezovi = bls.BrodskaLuka!.Vezovi.FindAll(x => x.Vrsta == vez);
            return vezovi.FindAll(x => ProvjeriBrodIVez(brod, x));
        }

        public static bool ProvjeriBrodIVez(Brod brod, Vez vez)
        {
            bool provjera = false;
            string[]? brodovi = null;
            switch (vez.Vrsta)
            {
                case "PU":
                    {
                        brodovi = Konstante.PutnickiBrodovi;
                        break;
                    }
                case "PO":
                    {
                        brodovi = Konstante.PoslovniBrodovi;
                        break;
                    }
                case "OS":
                    {
                        brodovi = Konstante.OstaliBrodovi;
                        break;
                    }
            }
            if (brodovi!.Contains(brod.Vrsta)
                    && vez.MaksDubina > brod.Gaz && vez.MaksSirina >= brod.Sirina && vez.MaksDuljina >= brod.Duljina)
            {
                provjera = true;
            }

            return provjera;
        }

        public static bool ProvjeriVezove(string vrsta)
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            BrodskaLuka? brodskaLuka = bls.BrodskaLuka;

            bool provjera = false;

            switch (vrsta)
            {
                case "PU":
                    {
                        provjera = brodskaLuka!.PutnickiVezovi > brodskaLuka.Vezovi.FindAll(x => x.Vrsta == "PU").Count;
                        break;
                    }
                case "PO":
                    {
                        provjera = brodskaLuka!.PoslovniVezovi > brodskaLuka.Vezovi.FindAll(x => x.Vrsta == "PO").Count;
                        break;
                    }
                case "OS":
                    {
                        provjera = brodskaLuka!.OstaliVezovi > brodskaLuka.Vezovi.FindAll(x => x.Vrsta == "OS").Count;
                        break;
                    }
            }

            return provjera;
        }

        public static Vez PronadjiNajekonomicnijiVez(List<Vez> vezovi)
        {
            return vezovi.Aggregate((min, sljedeci) =>
            {
                if (min.Volumen == sljedeci.Volumen)
                {
                    return min.CijenaPoSatu <= sljedeci.CijenaPoSatu ? min : sljedeci;
                }
                return min.Volumen < sljedeci.Volumen ? min : sljedeci;
            });
        }
    }
}
