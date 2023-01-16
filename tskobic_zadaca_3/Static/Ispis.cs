using tskobic_zadaca_3.Singleton;

namespace tskobic_zadaca_3.Static
{
    public static class Ispis
    {
        public static void GreskaPretvorbeUDouble(string redak, string celija, string putanja)
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            bls.Controller.SetModelState($"ERROR: Neispravan redak '{redak}' u datoteci '{putanja}'."
                 + $" Ćeliju '{celija}' nije moguće pretvoriti u broj tipa double. "
                   + $"Broj greške: {++BrodskaLukaSingleton.Instanca().BrojGreski}");
        }

        public static void GreskaPretvorbeUDane(string redak, string celija, string putanja)
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            bls.Controller.SetModelState($"ERROR: Neispravan redak '{redak}' u datoteci '{putanja}'."
                 + $" Ćeliju '{celija}' nije moguće pretvoriti u dane u tjednu. "
                   + $"Broj greške: {++BrodskaLukaSingleton.Instanca().BrojGreski}");
        }

        public static void GreskaPretvorbeUSate(string redak, string celija, string putanja)
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            bls.Controller.SetModelState($"ERROR: Neispravan redak '{redak}' u datoteci '{putanja}'."
                 + $" Ćeliju '{celija}' nije moguće pretvoriti u sate u danu. "
                   + $"Broj greške: {++BrodskaLukaSingleton.Instanca().BrojGreski}");
        }

        public static void GreskaPretvorbeUInt(string redak, string celija, string putanja)
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            bls.Controller.SetModelState($"ERROR: Neispravan redak '{redak}' u datoteci '{putanja}'."
                + $" Ćeliju '{celija}' nije moguće pretvoriti u broj tipa int. "
                + $"Broj greške: {++BrodskaLukaSingleton.Instanca().BrojGreski}");
        }

        public static void GreskaPretvorbeUDatum(string redak, string celija, string putanja)
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            bls.Controller.SetModelState($"ERROR: Neispravan redak '{redak}' u datoteci '{putanja}'."
                + $" Ćeliju '{celija}' nije moguće pretvoriti u datum. "
                + $"Broj greške: {++BrodskaLukaSingleton.Instanca().BrojGreski}");
        }

        public static void GreskaPretvorbeVezova(string redak, string celija, string putanja)
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            bls.Controller.SetModelState($"ERROR: Neispravan redak '{redak}' u datoteci '{putanja}'."
                + $" Ćeliju '{celija}' nije moguće pretvoriti u listu vezova. "
                + $"Broj greške: {++BrodskaLukaSingleton.Instanca().BrojGreski}");
        }

        public static void GreskaNeispravanInformativniRedak(string redak, string putanja)
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            bls.Controller.SetModelState($"ERROR: Neispravan prvi informativni redak '{redak}' u datoteci '{putanja}'."
            + $" Broj greške: {++BrodskaLukaSingleton.Instanca().BrojGreski}");
        }

        public static void GreskaBrojCelija(string redak, string putanja)
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            bls.Controller.SetModelState($"ERROR: Neispravan redak '{redak}' u datoteci '{putanja}'."
                + $" Broj ćelija je neispravan. "
                + $"Broj greške: {++BrodskaLukaSingleton.Instanca().BrojGreski}");
        }

        public static void GreskaNedozvoljenaVrsta(string redak, string celija, string putanja, string vrsta)
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            bls.Controller.SetModelState($"ERROR: Neispravan redak '{redak}' u datoteci '{putanja}'."
                + $" Ćelija '{celija}' ima {vrsta} nedozvoljene vrste."
                + $"Broj greške: {++BrodskaLukaSingleton.Instanca().BrojGreski}");
        }

        public static void GreskaNepostojeciZapis(string redak, string celija, string putanja, int id, string tip)
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            bls.Controller.SetModelState($"ERROR: Redak '{redak}' sa neispravnim zapisem u ćeliji '{celija}'"
                + $" u datoteci '{putanja}'."
                + $" {tip} sa proslijeđenim id-em {id} ne postoji."
                + $"Broj greške: {++BrodskaLukaSingleton.Instanca().BrojGreski}");
        }

        public static void GreskaInicijalizacije()
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            bls.Controller.SetModelState($"ERROR: Greška prilikom inicijalizacije."
                + $"Broj greške: {++BrodskaLukaSingleton.Instanca().BrojGreski}");
        }

        public static void VirtualniSat()
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            bls.Controller.SetModelState($"Virtualni sat: {bls.VirtualniSatOriginator.VirtualnoVrijeme:dd.MM.yyyy. HH:mm:ss}");
        }

        public static void GreskaArgumenti()
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            bls.Controller.SetModelState("ERROR: Neispravno uneseni argumenti!");
        }

        public static void GreskaNaredba()
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            bls.Controller.SetModelState("ERROR: Neispravno unesena naredba!");
        }

        public static void Vez(int id, string oznaka, string vrsta, string status)
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            bls.Controller.SetModelState(string.Format("|{0,10}|{1,-10}|{2,-10}|{3,-10}|", id, oznaka, vrsta, status));
        }

        public static void Vez(int redniBroj, int id, string oznaka, string vrsta, string status)
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            bls.Controller.SetModelState(string.Format("|{0,10}|{1,10}|{2,-10}|{3,-10}|{4,-10}|",
                redniBroj, id, oznaka, vrsta, status));
        }

        public static void ZaglavljeVez()
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            if (BrodskaLukaSingleton.Instanca().RedniBroj)
            {
                bls.Controller.SetModelState($"---------------------Status vezova---------------------");
                bls.Controller.SetModelState(string.Format("|{0,10}|{1,10}|{2,-10}|{3,-10}|{4,-10}|",
                "Redni broj", "ID", "Oznaka", "Vrsta", "Status"));
                bls.Controller.SetModelState($"-------------------------------------------------------");
            }
            else
            {
                bls.Controller.SetModelState($"----------------Status vezova----------------");
                bls.Controller.SetModelState(string.Format("|{0,10}|{1,-10}|{2,-10}|{3,-10}|", "ID", "Oznaka", "Vrsta", "Status"));
                bls.Controller.SetModelState($"---------------------------------------------");
            }
        }

        public static void Podnozje(int brojZapisa)
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            if (bls.RedniBroj)
            {
                bls.Controller.SetModelState($"-------------------------------------------------------");
                bls.Controller.SetModelState(string.Format("|{0,52}{1, 0}|", "Broj zapisa :", brojZapisa));
                bls.Controller.SetModelState($"-------------------------------------------------------");

            }
            else
            {
                bls.Controller.SetModelState($"---------------------------------------------");
                bls.Controller.SetModelState(string.Format("|{0,41}{1, 0}|", "Broj zapisa :", brojZapisa));
                bls.Controller.SetModelState($"---------------------------------------------");
            }
        }

        public static void ZaglavljeZauzetihVezova()
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            if (bls.RedniBroj)
            {
                bls.Controller.SetModelState($"-------------Broj zauzetih vezova po vrsti-------------");
                bls.Controller.SetModelState(string.Format("|{0,17}|{1,-17}|{2,17}|",
                "Redni broj", "Vrsta", "Broj zauzetih"));
                bls.Controller.SetModelState($"-------------------------------------------------------");
            }
            else
            {
                bls.Controller.SetModelState($"--------Broj zauzetih vezova po vrsti--------");
                bls.Controller.SetModelState(string.Format("|{0,-20}|{1,22}|",
                "Vrsta", "Broj zauzetih"));
                bls.Controller.SetModelState($"---------------------------------------------");
            }
        }

        public static void SumaZauzetihVezova(string vrsta, int brojZauzetih)
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            bls.Controller.SetModelState(string.Format("|{0,-20}|{1,22}|", vrsta, brojZauzetih));
        }

        public static void SumaZauzetihVezova(int redniBroj, string vrsta, int brojZauzetih)
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            bls.Controller.SetModelState(string.Format("|{0,17}|{1,-17}|{2,17}|",
                redniBroj, vrsta, brojZauzetih));
        }
    }
}
