using tskobic_zadaca_2.Singleton;

namespace tskobic_zadaca_2.Static
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

        public static void Vez(string id, string oznaka, string vrsta, string status)
        {
            Console.WriteLine(string.Format("|{0,10}|{1,10}|{2,10}|{3,10}|", id, oznaka, vrsta, status));
        }

        public static void Zaglavlje(string naslov)
        {
            Console.WriteLine($"----------------{naslov}----------------");
        }
    }
}
