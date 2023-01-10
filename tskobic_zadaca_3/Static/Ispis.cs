using tskobic_zadaca_3.Singleton;

namespace tskobic_zadaca_3.Static
{
    public static class Ispis
    {
        public static void GreskaPretvorbeUDouble(string redak, string celija, string putanja)
        {
            Console.WriteLine($"ERROR: Neispravan redak '{redak}' u datoteci '{putanja}'."
                 + $" Ćeliju '{celija}' nije moguće pretvoriti u broj tipa double. "
                   + $"Broj greške: {++BrodskaLukaSingleton.Instanca().BrojGreski}");
        }

        public static void GreskaPretvorbeUDane(string redak, string celija, string putanja)
        {
            Console.WriteLine($"ERROR: Neispravan redak '{redak}' u datoteci '{putanja}'."
                 + $" Ćeliju '{celija}' nije moguće pretvoriti u dane u tjednu. "
                   + $"Broj greške: {++BrodskaLukaSingleton.Instanca().BrojGreski}");
        }

        public static void GreskaPretvorbeUSate(string redak, string celija, string putanja)
        {
            Console.WriteLine($"ERROR: Neispravan redak '{redak}' u datoteci '{putanja}'."
                 + $" Ćeliju '{celija}' nije moguće pretvoriti u sate u danu. "
                   + $"Broj greške: {++BrodskaLukaSingleton.Instanca().BrojGreski}");
        }

        public static void GreskaPretvorbeUInt(string redak, string celija, string putanja)
        {
            Console.WriteLine($"ERROR: Neispravan redak '{redak}' u datoteci '{putanja}'."
                + $" Ćeliju '{celija}' nije moguće pretvoriti u broj tipa int. "
                + $"Broj greške: {++BrodskaLukaSingleton.Instanca().BrojGreski}");
        }

        public static void GreskaPretvorbeUDatum(string redak, string celija, string putanja)
        {
            Console.WriteLine($"ERROR: Neispravan redak '{redak}' u datoteci '{putanja}'."
                + $" Ćeliju '{celija}' nije moguće pretvoriti u datum. "
                + $"Broj greške: {++BrodskaLukaSingleton.Instanca().BrojGreski}");
        }

        public static void GreskaPretvorbeVezova(string redak, string celija, string putanja)
        {
            Console.WriteLine($"ERROR: Neispravan redak '{redak}' u datoteci '{putanja}'."
                + $" Ćeliju '{celija}' nije moguće pretvoriti u listu vezova. "
                + $"Broj greške: {++BrodskaLukaSingleton.Instanca().BrojGreski}");
        }

        public static void GreskaNeispravanInformativniRedak(string redak, string putanja)
        {
            Console.WriteLine($"ERROR: Neispravan prvi informativni redak '{redak}' u datoteci '{putanja}'."
                + $" Broj greške: {++BrodskaLukaSingleton.Instanca().BrojGreski}");
        }

        public static void GreskaBrojCelija(string redak, string putanja)
        {
            Console.WriteLine($"ERROR: Neispravan redak '{redak}' u datoteci '{putanja}'."
                + $" Broj ćelija je neispravan. "
                + $"Broj greške: {++BrodskaLukaSingleton.Instanca().BrojGreski}");
        }

        public static void GreskaNedozvoljenaVrsta(string redak, string celija, string putanja, string vrsta)
        {
            Console.WriteLine($"ERROR: Neispravan redak '{redak}' u datoteci '{putanja}'."
                + $" Ćelija '{celija}' ima {vrsta} nedozvoljene vrste."
                + $"Broj greške: {++BrodskaLukaSingleton.Instanca().BrojGreski}");
        }

        public static void GreskaNepostojeciZapis(string redak, string celija, string putanja, int id, string tip)
        {
            Console.WriteLine($"ERROR: Redak '{redak}' sa neispravnim zapisem u ćeliji '{celija}'"
                + $" u datoteci '{putanja}'."
                + $" {tip} sa proslijeđenim id-em {id} ne postoji."
                + $"Broj greške: {++BrodskaLukaSingleton.Instanca().BrojGreski}");
        }

        public static void GreskaInicijalizacije()
        {
            Console.WriteLine($"ERROR: Greška prilikom inicijalizacije."
                + $"Broj greške: {++BrodskaLukaSingleton.Instanca().BrojGreski}");
        }

        public static void VirtualniSat()
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            Console.WriteLine($"Virtualni sat: {bls.VirtualniSat.VirtualnoVrijeme:dd.MM.yyyy. HH:mm:ss}");
        }

        public static void GreskaArgumenti()
        {
            Console.WriteLine("ERROR: Neispravno uneseni argumenti!");
        }

        public static void GreskaNaredba()
        {
            Console.WriteLine("ERROR: Neispravno unesena naredba!");
        }

        public static void Vez(int id, string oznaka, string vrsta, string status)
        {
            Console.WriteLine(string.Format("|{0,10}|{1,-10}|{2,-10}|{3,-10}|", id, oznaka, vrsta, status));
        }

        public static void Vez(int redniBroj, int id, string oznaka, string vrsta, string status)
        {
            Console.WriteLine(string.Format("|{0,10}|{1,10}|{2,-10}|{3,-10}|{4,-10}|",
                redniBroj, id, oznaka, vrsta, status));
        }

        public static void ZaglavljeVez()
        {

            if (BrodskaLukaSingleton.Instanca().RedniBroj)
            {
                Console.WriteLine($"---------------------Status vezova---------------------");
                Console.WriteLine(string.Format("|{0,10}|{1,10}|{2,-10}|{3,-10}|{4,-10}|",
                "Redni broj", "ID", "Oznaka", "Vrsta", "Status"));
                Console.WriteLine($"-------------------------------------------------------");
            }
            else
            {
                Console.WriteLine($"----------------Status vezova----------------");
                Console.WriteLine(string.Format("|{0,10}|{1,-10}|{2,-10}|{3,-10}|", "ID", "Oznaka", "Vrsta", "Status"));
                Console.WriteLine($"---------------------------------------------");
            }
        }

        public static void Podnozje(int brojZapisa)
        {
            if (BrodskaLukaSingleton.Instanca().RedniBroj)
            {
                Console.WriteLine($"-------------------------------------------------------");
                Console.WriteLine(string.Format("|{0,52}{1, 0}|", "Broj zapisa :", brojZapisa));
                Console.WriteLine($"-------------------------------------------------------");

            }
            else
            {
                Console.WriteLine($"---------------------------------------------");
                Console.WriteLine(string.Format("|{0,41}{1, 0}|", "Broj zapisa :", brojZapisa));
                Console.WriteLine($"---------------------------------------------");
            }
        }

        public static void ZaglavljeZauzetihVezova()
        {

            if (BrodskaLukaSingleton.Instanca().RedniBroj)
            {
                Console.WriteLine($"-------------Broj zauzetih vezova po vrsti-------------");
                Console.WriteLine(string.Format("|{0,17}|{1,-17}|{2,17}|",
                "Redni broj", "Vrsta", "Broj zauzetih"));
                Console.WriteLine($"-------------------------------------------------------");
            }
            else
            {
                Console.WriteLine($"--------Broj zauzetih vezova po vrsti--------");
                Console.WriteLine(string.Format("|{0,-20}|{1,22}|",
                "Vrsta", "Broj zauzetih"));
                Console.WriteLine($"---------------------------------------------");
            }
        }

        public static void SumaZauzetihVezova(string vrsta, int brojZauzetih)
        {
            Console.WriteLine(string.Format("|{0,-20}|{1,22}|", vrsta, brojZauzetih));
        }

        public static void SumaZauzetihVezova(int redniBroj, string vrsta, int brojZauzetih)
        {
            Console.WriteLine(string.Format("|{0,17}|{1,-17}|{2,17}|",
                redniBroj, vrsta, brojZauzetih));
        }
    }
}
