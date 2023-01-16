using System.Globalization;
using System.Text.RegularExpressions;
using tskobic_zadaca_3.ChainOfResponsibility;
using tskobic_zadaca_3.Composite;
using tskobic_zadaca_3.FactoryMethod;
using tskobic_zadaca_3.Modeli;
using tskobic_zadaca_3.MVC;
using tskobic_zadaca_3.Observer;
using tskobic_zadaca_3.Singleton;
using tskobic_zadaca_3.Static;
using tskobic_zadaca_3.Visitor;

namespace tskobic_zadaca_3
{
    public class Program
    {
        private static bool KrajPrograma { get; set; } = false;

        #region Metode
        private static bool Inicijalizacija(List<Group> grupe)
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            UcitajPodatke(new LukaCreator(), grupe, "-l");
            if (bls.BrodskaLuka == null)
            {
                return false;
            }

            UcitajPodatke(new MoloviCreator(), grupe, "-m");
            List<IComponent> molovi = bls.BrodskaLuka!.Find(c => c is Mol).ToList();
            if (molovi.Count == 0)
            {
                return false;
            }

            UcitajPodatke(new VezoviCreator(), grupe, "-v");
            List<IComponent> vezovi = bls.BrodskaLuka!.Find(c => c is Vez);
            if (vezovi.Count == 0)
            {
                return false;
            }

            UcitajPodatke(new MoloviVezoviCreator(), grupe, "-mv");
            vezovi = bls.BrodskaLuka!.Find(c => c is Vez);
            if (!vezovi.Exists(v => ((Vez)v).IdMol != null))
            {
                return false;
            }
            bls.BrodskaLuka.RemoveAll(c => c is Vez vez && vez.IdMol == null);
            vezovi = bls.BrodskaLuka!.Find(c => c is Vez);

            UcitajPodatke(new KanaliCreator(), grupe, "-k");
            if (bls.BrodskaLuka.Kanali.Count == 0)
            {
                return false;
            }

            UcitajPodatke(new BrodoviCreator(), grupe, "-b");
            if (bls.BrodskaLuka.Brodovi.Count == 0)
            {
                return false;
            }

            UcitajPodatke(new RasporediCreator(), grupe, "-r");
            return true;
        }

        private static void UcitajPodatke(Creator creator, List<Group> grupe, string opcija)
        {
            Group? grupa = grupe.Find(x => x.Value.StartsWith(opcija));
            if (grupa != null)
            {
                string[] podaci = grupa!.Value.Split(" ");
                creator.ProcitajPodatke(podaci[1]);
            }
        }

        private static void InicijalizacijaTerminala(List<Group> grupe)
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            Terminal terminal = bls.Terminal;

            int brojRedaka = int.Parse(grupe.First(g => g.Value.StartsWith("-br")).Value[4..]);
            terminal.BrojRedaka = brojRedaka;

            string ulogeEkrana = grupe.First(g => g.Value.StartsWith("-pd")).Value[4..];
            terminal.RadniDioPrvi = ulogeEkrana.StartsWith('R');

            string omjer = grupe.First(g => g.Value.StartsWith("-vt")).Value[4..];
            double prviDioEkrana = 0;
            if (omjer.StartsWith('5'))
            {
                prviDioEkrana = 0.5;
            }
            else if (omjer.StartsWith('2'))
            {
                prviDioEkrana = 0.25;
            }
            else if (omjer.StartsWith('7'))
            {
                prviDioEkrana = 0.75;
            }
            if (terminal.RadniDioPrvi)
            {
                terminal.RadniDioPocetak = 1;
                terminal.RadniDioKraj = (int)Math.Round(brojRedaka * prviDioEkrana) - 1;
                terminal.Sredina = (int)Math.Round(brojRedaka * prviDioEkrana);
                terminal.GreskePocetak = terminal.Sredina + 2;
                terminal.GreskeKraj = brojRedaka;
            }
            else
            {
                terminal.GreskePocetak = 1;
                terminal.GreskeKraj = (int)Math.Round(brojRedaka * prviDioEkrana) - 1;
                terminal.Sredina = (int)Math.Round(brojRedaka * prviDioEkrana);
                terminal.RadniDioPocetak = terminal.Sredina + 2;
                terminal.RadniDioKraj = brojRedaka;
            }
        }

        private static bool ProvjeraArgumentaBR(Group grupa)
        {
            bool provjera = int.TryParse(grupa!.Value.Split(" ")[1], out int brojRedaka);
            return provjera && brojRedaka >= 24 && brojRedaka <= 80;
        }

        private static void PrivezRezerviranogBroda(int idBrod)
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            Brod? brod = bls.BrodskaLuka!.Brodovi.Find(x => x.ID == idBrod);
            if (brod == null)
            {
                bls.Controller.SetModelState($"Brod s proslijeđenim ID-em {idBrod} ne postoji.");
                return;
            }
            List<Kanal> kanali = bls.BrodskaLuka.Kanali;
            Kanal? kanal = kanali.Find(x => x.Observers.Contains(brod));
            if (kanal == null)
            {
                bls.Controller.SetModelState($"Brod s ID-em {idBrod} mora biti u komunikaciji "
                    + $"s kapetanijom da bi zatražio privez.");
                return;
            }
            DateTime datum = bls.VirtualniSatOriginator.VirtualnoVrijeme;
            TimeOnly vrijeme = TimeOnly.FromTimeSpan(datum.TimeOfDay);
            DayOfWeek dan = datum.DayOfWeek;
            Privez? postojeciPrivez = bls.BrodskaLuka.Privezi.Find(x => x.IdBrod == idBrod
                && x.VrijemeOd <= datum && datum <= x.VrijemeDo);
            string poruka = "";
            if (postojeciPrivez == null)
            {
                Raspored? raspored = bls.BrodskaLuka.Rasporedi.Find(x => x.IdBrod == idBrod
                    && x.DaniUTjednu.Contains(dan) && x.VrijemeOd <= vrijeme && vrijeme <= x.VrijemeDo);
                if (raspored != null)
                {
                    DateTime vrijemeDo = datum.Date.Add(raspored.VrijemeDo.ToTimeSpan());
                    Privez privez = new Privez(raspored.IdVez, idBrod, datum, vrijemeDo);
                    bls.BrodskaLuka.Privezi.Add(privez);
                    bls.BrodskaLuka.Dnevnik.Add(new Zapis(VrstaZahtjeva.ZD, idBrod, false, datum, vrijemeDo));
                    ZapisiPoruku(kanal, $"Brodu s ID-em {brod.ID} odobren privez na vez {raspored.IdVez}.");
                    bls.Controller.SetModelState("Naredba uspješna.");
                    return;
                }
                else
                {
                    Rezervacija? rezervacija = bls.BrodskaLuka.Rezervacije.Find(x => x.IdBrod == idBrod
                        && x.DatumOd.Date.Equals(datum.Date) && TimeOnly.FromDateTime(x.DatumOd) <= vrijeme
                        && vrijeme <= TimeOnly.FromDateTime(x.DatumOd).AddHours(x.SatiTrajanja));
                    if (rezervacija != null)
                    {
                        DateTime vrijemeDo = rezervacija.DatumOd.AddHours(rezervacija.SatiTrajanja);
                        Privez privez = new Privez(rezervacija.IdVez, idBrod,
                            datum, vrijemeDo);
                        bls.BrodskaLuka.Privezi.Add(privez);
                        bls.BrodskaLuka.Dnevnik.Add(new Zapis(VrstaZahtjeva.ZD, idBrod, false, datum, vrijemeDo));
                        ZapisiPoruku(kanal, $"Brodu s ID-em {brod.ID} odobren privez na vez {rezervacija.IdVez}.");
                        bls.Controller.SetModelState("Naredba uspješna.");
                        return;
                    }
                    else
                    {
                        poruka = $"Brodu s ID-em {brod.ID} odbijen privez na vez, ne postoji rezervacija.";
                    }
                }
            }
            else
            {
                poruka = $"Brodu s ID-em {brod.ID} odbijen privez na vez, već je privezan.";
            }
            bls.BrodskaLuka.Dnevnik.Add(new Zapis(VrstaZahtjeva.ZD, idBrod, true));
            ZapisiPoruku(kanal, $"{poruka}");
            bls.Controller.SetModelState(poruka);
        }

        private static void PrivezSlobodnogBroda(int idBrod, int trajanje)
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            Brod? brod = bls.BrodskaLuka!.Brodovi.Find(x => x.ID == idBrod);
            if (brod == null)
            {
                bls.Controller.SetModelState($"Brod s proslijeđenim ID-em {idBrod} ne postoji.");
                return;
            }
            List<Kanal> kanali = bls.BrodskaLuka.Kanali;
            Kanal? kanal = kanali.Find(x => x.Observers.Contains(brod));
            if (kanal == null)
            {
                bls.Controller.SetModelState($"Brod s ID-em {idBrod} mora biti u komunikaciji "
                    + $"s kapetanijom da bi zatražio privez.");
                return;
            }
            DateTime datum = bls.VirtualniSatOriginator.VirtualnoVrijeme;
            TimeOnly vrijeme = TimeOnly.FromTimeSpan(datum.TimeOfDay);
            DayOfWeek dan = datum.DayOfWeek;
            Privez? postojeciPrivez = bls.BrodskaLuka.Privezi.Find(x => x.IdBrod == idBrod
                && x.VrijemeOd <= datum && datum <= x.VrijemeDo);
            string poruka = "";
            if (postojeciPrivez == null)
            {
                List<Vez> vezovi = Utils.PronadjiVezove(brod);
                if (vezovi.Count > 0)
                {
                    List<Vez> fVezoviPrivezi = vezovi.FindAll(vez => bls.BrodskaLuka.Privezi.Any(x => x.IdVez == vez.ID
                        && x.VrijemeOd <= datum.AddHours(trajanje) && datum <= x.VrijemeDo));

                    List<Vez> fVezoviRasporedi = vezovi.FindAll(vez => bls.BrodskaLuka.Rasporedi
                        .Any(x => x.IdVez == vez.ID
                        && x.DaniUTjednu.Contains(dan)
                        && x.VrijemeOd <= vrijeme.AddHours(trajanje) && vrijeme <= x.VrijemeDo));

                    List<Vez> fVezoviRezervacije = vezovi.FindAll(vez => bls.BrodskaLuka.Rezervacije
                        .Any(x => x.IdVez == vez.ID
                        && x.DatumOd.Date == datum.Date
                        && TimeOnly.FromTimeSpan(x.DatumOd.TimeOfDay) <= vrijeme.AddHours(trajanje)
                        && vrijeme <= TimeOnly.FromTimeSpan(x.DatumOd.TimeOfDay).AddHours(x.SatiTrajanja)));

                    List<Vez> slobodniVezovi = vezovi.Except(fVezoviPrivezi)
                        .Except(fVezoviRasporedi).Except(fVezoviRezervacije).ToList();

                    if (slobodniVezovi.Count > 0)
                    {
                        Vez vez = Utils.PronadjiNajekonomicnijiVez(slobodniVezovi);
                        Privez privez = new Privez(vez.ID, idBrod, datum, datum.AddHours(trajanje));
                        bls.BrodskaLuka.Privezi.Add(privez);
                        bls.BrodskaLuka.Dnevnik.Add(new Zapis(VrstaZahtjeva.ZP, idBrod,
                            false, datum, datum.AddHours(trajanje)));
                        ZapisiPoruku(kanal, $"Brodu s ID-em {brod.ID} odobren privez na vez {vez.ID}.");
                        bls.Controller.SetModelState("Naredba uspješna.");
                        return;
                    }
                    else
                    {
                        poruka = $"Brodu s ID-em {brod.ID} odbijen privez na vez, ne postoji slobodan vez";
                    }
                }
                else
                {
                    poruka = $"Brodu s ID-em {brod.ID} odbijen privez na vez, ne postoji odgovarajući vez.";
                }
            }
            else
            {
                poruka = $"Brodu s ID-em {brod.ID} odbijen privez na vez, brod je već privezan.";
            }
            bls.BrodskaLuka.Dnevnik.Add(new Zapis(VrstaZahtjeva.ZP, idBrod, true));
            ZapisiPoruku(kanal, $"{poruka}");
            bls.Controller.SetModelState(poruka);
        }

        private static void IspisStatusaVezova()
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            List<Vez> vezovi = bls.BrodskaLuka!.Find(c => c is Vez).Cast<Vez>().ToList();

            DateTime datum = bls.VirtualniSatOriginator.VirtualnoVrijeme;
            TimeOnly vrijeme = TimeOnly.FromTimeSpan(datum.TimeOfDay);
            DayOfWeek dan = datum.DayOfWeek;

            List<Vez> fVezoviPrivezi = vezovi.FindAll(vez => bls.BrodskaLuka.Privezi.Any(x => x.IdVez == vez.ID
                && x.VrijemeOd <= datum && datum <= x.VrijemeDo));

            List<Vez> fVezoviRasporedi = vezovi.FindAll(vez => bls.BrodskaLuka.Rasporedi.Any(x => x.IdVez == vez.ID
                 && x.DaniUTjednu.Contains(dan) && x.VrijemeOd <= vrijeme && vrijeme <= x.VrijemeDo));

            List<Vez> fVezoviRezervacije = vezovi.FindAll(vez => bls.BrodskaLuka.Rezervacije.Any(x => x.IdVez == vez.ID
                && x.DatumOd.Date == datum.Date
                && TimeOnly.FromTimeSpan(x.DatumOd.TimeOfDay) <= vrijeme
                && vrijeme <= TimeOnly.FromTimeSpan(x.DatumOd.TimeOfDay).AddHours(x.SatiTrajanja)));

            List<Vez> zauzetiVezovi = fVezoviPrivezi.Union(fVezoviRasporedi)
                .Union(fVezoviRezervacije).Distinct().ToList();
            List<Vez> slobodniVezovi = vezovi.Except(zauzetiVezovi).ToList();

            if (bls.Zaglavlje)
            {
                Ispis.ZaglavljeVez();
            }
            IspisListeVezova(slobodniVezovi, "Slobodan");
            IspisListeVezova(zauzetiVezovi, "Zauzet");
            if (bls.Podnozje)
            {
                int brojZapisa = slobodniVezovi.Count + zauzetiVezovi.Count;
                Ispis.Podnozje(brojZapisa);
            }
        }

        private static void IspisListeVezova(List<Vez> vezovi, string status)
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();

            for (int i = 0; i < vezovi.Count; i++)
            {
                if (bls.RedniBroj)
                {
                    Ispis.Vez(i + 1, vezovi[i].ID, vezovi[i].Oznaka, vezovi[i].Vrsta, status);
                }
                else
                {
                    Ispis.Vez(vezovi[i].ID, vezovi[i].Oznaka, vezovi[i].Vrsta, status);

                }
            }
        }

        private static void UredjenjeIspisa(string unos)
        {
            List<string> opcije = unos.Split(" ").ToList();
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            if (opcije.Count == 1)
            {
                bls.Zaglavlje = false;
                bls.Podnozje = false;
                bls.RedniBroj = false;
            }
            else
            {
                bls.Zaglavlje = opcije.Contains("Z");
                bls.Podnozje = opcije.Contains("P");
                bls.RedniBroj = opcije.Contains("RB");
            }
        }

        //TODO Implementacija V naredbe
        private static void IspisVezovaVrste(string vrsta, string status, string datumOd, string datumDo)
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();

            Utils.ProvjeriPretvorbuUDatum(datumOd, out DateTime intervalOd);
            Utils.ProvjeriPretvorbuUDatum(datumDo, out DateTime intervalDo);
        }

        private static void IspisZauzetihVezova(string ulaz)
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            List<IComponent> vezovi = bls.BrodskaLuka!.Find(c => c is Vez);

            DateTime.TryParseExact(ulaz, "dd.MM.yyyy. HH:mm", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out DateTime datum);
            TimeOnly vrijeme = TimeOnly.FromTimeSpan(datum.TimeOfDay);
            DayOfWeek dan = datum.DayOfWeek;

            List<Privez> privezi = bls.BrodskaLuka.Privezi;
            List<Raspored> rasporedi = bls.BrodskaLuka.Rasporedi;
            List<Rezervacija> rezervacije = bls.BrodskaLuka.Rezervacije;

            List<IElement> elementi = privezi.ToList<IElement>().Concat(rasporedi).Concat(rezervacije).ToList();

            List<Vez> zauzetiVezovi = new List<Vez>();
            IElementVisitor elementVisitor = new ElementVisitor(datum);

            foreach (IElement element in elementi)
            {
                int? id = element.Accept(elementVisitor);
                if (id != null)
                {
                    Vez? vez = (Vez?)vezovi.Find(x => x.GetId() == id);
                    if (vez != null && !zauzetiVezovi.Contains(vez))
                    {
                        zauzetiVezovi.Add(vez);
                    }
                }
            }
            if (zauzetiVezovi.Count > 0)
            {
                SumarnoZbrajanjeVezova(zauzetiVezovi);
                return;
            }
            bls.Controller.SetModelState("Svi vezovi su slobodni.");
        }

        public static void SumarnoZbrajanjeVezova(List<Vez> vezovi)
        {
            int putnickiVezovi = 0;
            int poslovniVezovi = 0;
            int ostaliVezovi = 0;
            IVezVisitor vezVisitor = new VezVisitor();

            foreach (IVez vez in vezovi)
            {
                string vrsta = vez.Accept(vezVisitor);
                switch (vrsta)
                {
                    case "PU":
                        {
                            putnickiVezovi++;
                            break;
                        }
                    case "PO":
                        {
                            poslovniVezovi++;
                            break;
                        }
                    case "OS":
                        {
                            ostaliVezovi++;
                            break;
                        }
                    default:
                        break;
                }
            }
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();

            if (bls.Zaglavlje)
            {
                Ispis.ZaglavljeZauzetihVezova();
            }
            IspisSumeZauzetihVezova(putnickiVezovi, poslovniVezovi, ostaliVezovi);
            if (bls.Podnozje)
            {
                int brojZapisa = 3;
                Ispis.Podnozje(brojZapisa);
            }
        }

        private static void IspisSumeZauzetihVezova(int putnicki, int poslovni, int ostali)
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            if (bls.RedniBroj)
            {
                Ispis.SumaZauzetihVezova(1, "PU", putnicki);
                Ispis.SumaZauzetihVezova(2, "PO", poslovni);
                Ispis.SumaZauzetihVezova(3, "OS", ostali);
            }
            else
            {
                Ispis.SumaZauzetihVezova("PU", putnicki);
                Ispis.SumaZauzetihVezova("PO", poslovni);
                Ispis.SumaZauzetihVezova("OS", ostali);
            }
        }

        private static void SpajanjeNaKanal(int idBrod, int frekvencija, bool odjava)
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            List<Kanal> kanali = bls.BrodskaLuka!.Kanali;

            Kanal? kanal = kanali.Find(x => x.Frekvencija == frekvencija);
            if (kanal == null)
            {
                bls.Controller.SetModelState("Naredbna neuspješna, uneseni kanal ne postoji.");
                return;
            }
            Brod? brod = bls.BrodskaLuka.Brodovi.Find(x => x.ID == idBrod);
            if (brod == null)
            {
                bls.Controller.SetModelState("Naredba neuspješna, brod s proslijeđenim ID-em ne postoji.");
                return;
            }
            if (!odjava)
            {
                if (kanal.Zauzet())
                {
                    bls.Controller.SetModelState("Naredba neuspješna, maksimalan broj komunikacija na kanalu.");
                    return;
                }
                if (kanali.Exists(x => x.Observers.Contains(brod)))
                {
                    bls.Controller.SetModelState("Naredba neuspješna, brod već ima aktivnu komunikaciju");
                    return;
                }
                kanal.Attach(brod);
                bls.Controller.SetModelState("Naredba uspješna");
            }
            else if (odjava)
            {
                if (kanal.Observers.Contains(brod))
                {
                    DateTime vrijeme = bls.VirtualniSatOriginator.VirtualnoVrijeme;
                    Privez? privez = bls.BrodskaLuka.Privezi.Find(x => x.IdBrod == brod.ID
                        && x.VrijemeOd <= vrijeme && vrijeme <= x.VrijemeDo);
                    if (privez != null)
                    {
                        privez.VrijemeDo = vrijeme;
                    }
                    kanal.Detach(brod);
                    bls.Controller.SetModelState($"Naredba uspješna, brod {idBrod} je odjavljen sa kanal {frekvencija}.");
                }
                else
                {
                    bls.Controller.SetModelState($"Naredba neuspješna, ne postoji aktivna komunikacija "
                        + $"na kanalu {frekvencija} za brod {idBrod}.");
                }
            }
        }

        public static void ZapisiPoruku(int idBrod, string poruka)
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            Brod? brod = bls.BrodskaLuka!.Brodovi.Find(x => x.ID == idBrod);
            if (brod == null)
            {
                return;
            }
            List<Kanal> kanali = bls.BrodskaLuka.Kanali;
            Kanal? kanal = kanali.Find(x => x.Observers.Contains(brod));
            if (kanal != null)
            {
                kanal.SetState(poruka);
            }
        }

        public static void ZapisiPoruku(Kanal kanal, string poruka)
        {
            kanal.SetState(poruka);
        }

        public static void StatusBroda(int idBrod)
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            Brod? brod = bls.BrodskaLuka!.Brodovi.Find(x => x.ID == idBrod);
            if (brod == null)
            {
                bls.Controller.SetModelState($"Brod s proslijeđenim ID-em {idBrod} ne postoji.");
                return;
            }

            IHandler handler = DohvatiLanacHandlera();
            handler.Handle(idBrod);
        }

        public static IHandler DohvatiLanacHandlera()
        {
            IHandler privezHandler = new PrivezHandler();
            IHandler rezervacijaHandler = new RezervacijaHandler();
            IHandler rasporedHandler = new RasporedHandler();

            privezHandler.SetNext(rezervacijaHandler);
            rezervacijaHandler.SetNext(rasporedHandler);

            return privezHandler;
        }
        #endregion

        #region Main metoda
        static void Main(string[] args)
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            Model model = new Model();
            View view = new View();
            model.Attach(view);
            bls.Controller.AddModel(model);
            bls.Controller.AddView(view);

            Regex rg = new Regex(Konstante.UlazniArgumenti);
            Match match = rg.Match(string.Join(" ", args));
            List<Group> initGrupe = new List<Group>();
            List<Group> terminalGrupe = new List<Group>();

            if (match.Success)
            {
                for (int i = 1; i < match.Groups.Count; i++)
                {
                    if (match.Groups[i].Success)
                    {
                        if (match.Groups[i].Value.StartsWith("-br")
                            || match.Groups[i].Value.StartsWith("-vt")
                            || match.Groups[i].Value.StartsWith("-pd"))
                        {
                            terminalGrupe.Add(match.Groups[i]);
                        }
                        initGrupe.Add(match.Groups[i]);
                    }
                }
                if (!ProvjeraArgumentaBR(terminalGrupe.Find(x => x.Value.StartsWith("-br"))!))
                {
                    Ispis.GreskaArgumenti();
                    return;
                }
                InicijalizacijaTerminala(terminalGrupe);
                Terminal terminal = bls.Terminal;
                bls.Controller.SetModelState(Konstante.UNICODE_ESC + "2J");

                bls.Controller.SetModelState(Konstante.UNICODE_ESC + $"1d");
                bls.Controller.SetModelState(Konstante.UNICODE_ESC + $"{terminal.Sredina}d");
                for (int i = 0; i < 80; i++)
                {
                    bls.Controller.SetModelState("=");
                }
                bls.Controller.SetModelState(Konstante.UNICODE_ESC + $"{terminal.GreskePocetak};{terminal.GreskeKraj}r");
                bls.Controller.SetModelState(Konstante.UNICODE_ESC + $"{terminal.GreskePocetak}d");
                if (!Inicijalizacija(initGrupe))
                {
                    Ispis.GreskaInicijalizacije();
                    return;
                }
                bls.VirtualniSatOriginator.StvarnoVrijeme = DateTime.Now;

                bls.Controller.SetModelState(Konstante.UNICODE_ESC + $"{terminal.RadniDioPocetak};{terminal.RadniDioKraj}r");
                bls.Controller.SetModelState(Konstante.UNICODE_ESC + $"{terminal.RadniDioPocetak}d");
                while (!KrajPrograma)
                {
                    bls.Controller.SetModelState("Unesite komandu:");
                    switch (Console.ReadLine())
                    {
                        case "I":
                            {
                                bls.VirtualniSatOriginator.IzvrsiVirtualniPomak();
                                Ispis.VirtualniSat();
                                IspisStatusaVezova();
                                break;
                            }
                        case string ulaz when new Regex(Konstante.IspisVezova).IsMatch(ulaz):
                            {
                                bls.VirtualniSatOriginator.IzvrsiVirtualniPomak();
                                Ispis.VirtualniSat();
                                string[] podaci = ulaz.Split(" ");
                                if (Konstante.VrsteVezova.Contains(podaci[1]))
                                {
                                    //TODO V naredba
                                    //IspisVezovaVrste(podaci[1], podaci[2],
                                    //    $"{podaci[3]} {podaci[4]}", $"{podaci[5]} {podaci[6]}");
                                }
                                break;
                            }
                        case string ulaz when new Regex(Konstante.VirtualnoVrijeme).IsMatch(ulaz):
                            {
                                bls.VirtualniSatOriginator.StvarnoVrijeme = DateTime.Now;
                                bls.VirtualniSatOriginator.VirtualnoVrijeme = DateTime.Parse(ulaz.Substring(3));
                                Ispis.VirtualniSat();
                                break;
                            }
                        case string ulaz when new Regex(Konstante.ZahtjevRezervacije).IsMatch(ulaz):
                            {
                                Creator creator = new RezervacijeCreator();
                                bls.VirtualniSatOriginator.IzvrsiVirtualniPomak();
                                Ispis.VirtualniSat();
                                creator.ProcitajPodatke(ulaz.Substring(3));
                                break;
                            }
                        case string ulaz when new Regex(Konstante.ZahtjevRezPriveza).IsMatch(ulaz):
                            {
                                bls.VirtualniSatOriginator.IzvrsiVirtualniPomak();
                                Ispis.VirtualniSat();
                                int idBrod = int.Parse(ulaz.Substring(3));
                                ZapisiPoruku(idBrod, ulaz);
                                PrivezRezerviranogBroda(idBrod);
                                break;
                            }
                        case string ulaz when new Regex(Konstante.ZahtjevSlobPriveza).IsMatch(ulaz):
                            {
                                bls.VirtualniSatOriginator.IzvrsiVirtualniPomak();
                                Ispis.VirtualniSat();
                                string[] podaci = ulaz.Split(" ");
                                int idBrod = int.Parse(podaci[1]);
                                int trajanje = int.Parse(podaci[2]);
                                ZapisiPoruku(idBrod, ulaz);
                                PrivezSlobodnogBroda(idBrod, trajanje);
                                break;
                            }
                        case string ulaz when new Regex(Konstante.SpajanjeNaKanal).IsMatch(ulaz):
                            {
                                bls.VirtualniSatOriginator.IzvrsiVirtualniPomak();
                                Ispis.VirtualniSat();
                                string[] opcije = ulaz.Split(" ");
                                int idBrod = int.Parse(opcije[1]);
                                int frekvencija = int.Parse(opcije[2]);
                                if (opcije.Length == 3)
                                {
                                    SpajanjeNaKanal(idBrod, frekvencija, false);
                                }
                                else if (opcije.Length == 4)
                                {
                                    ZapisiPoruku(idBrod, ulaz);
                                    SpajanjeNaKanal(idBrod, frekvencija, true);
                                }
                                break;
                            }
                        case string ulaz when new Regex(Konstante.UredjenjeIspisa).IsMatch(ulaz):
                            {
                                bls.VirtualniSatOriginator.IzvrsiVirtualniPomak();
                                Ispis.VirtualniSat();
                                UredjenjeIspisa(ulaz);
                                break;
                            }
                        case string ulaz when new Regex(Konstante.IspisZauzetihVezova).IsMatch(ulaz):
                            {
                                bls.VirtualniSatOriginator.IzvrsiVirtualniPomak();
                                Ispis.VirtualniSat();
                                IspisZauzetihVezova(ulaz.Substring(3));
                                break;
                            }
                        case string ulaz when new Regex(Konstante.StatusBroda).IsMatch(ulaz):
                            {
                                bls.VirtualniSatOriginator.IzvrsiVirtualniPomak();
                                Ispis.VirtualniSat();
                                string[] podaci = ulaz.Split(" ");
                                int idBrod = int.Parse(podaci[1]);
                                StatusBroda(idBrod);
                                break;
                            }
                        case string ulaz when new Regex(Konstante.SpremanjePostojecegStanja).IsMatch(ulaz):
                            {
                                bls.VirtualniSatOriginator.IzvrsiVirtualniPomak();
                                Ispis.VirtualniSat();
                                string naziv = ulaz.Substring(4).Trim('"');
                                if (bls.CareTaker.Get(naziv) != null)
                                {
                                    bls.Controller.SetModelState("Naredba neuspješna, " +
                                        "već postoji prethodno spremljeno stanje s tim nazivom!");
                                }
                                else
                                {
                                    bls.CareTaker.Add(bls.VirtualniSatOriginator.SaveStateToMemento(naziv));
                                    bls.Controller.SetModelState("Naredba uspješna");
                                }
                                break;
                            }
                        case string ulaz when new Regex(Konstante.VracanjeStanjaVezova).IsMatch(ulaz):
                            {
                                bls.VirtualniSatOriginator.IzvrsiVirtualniPomak();
                                Ispis.VirtualniSat();
                                string naziv = ulaz.Substring(4).Trim('"');
                                Memento.Memento? memento = bls.CareTaker.Get(naziv);
                                if (memento == null)
                                {
                                    bls.Controller.SetModelState("Naredba neuspješna, " +
                                        "ne postoji prethodno spremljeno stanje s tim nazivom!");
                                }
                                else
                                {
                                    bls.VirtualniSatOriginator.StvarnoVrijeme = DateTime.Now;
                                    bls.VirtualniSatOriginator.VirtualnoVrijeme = memento.VirtualnoVrijeme;
                                    bls.Controller.SetModelState("Naredba uspješna");
                                    Ispis.VirtualniSat();
                                }
                                break;
                            }
                        case "Q":
                            {
                                bls.VirtualniSatOriginator.IzvrsiVirtualniPomak();
                                Ispis.VirtualniSat();
                                KrajPrograma = true;
                                break;
                            }
                        default:
                            {
                                bls.VirtualniSatOriginator.IzvrsiVirtualniPomak();
                                Ispis.VirtualniSat();
                                Ispis.GreskaNaredba();
                                break;
                            }
                    }
                }
            }
            else
            {
                Ispis.GreskaArgumenti();
            }
            bls.Controller.SetModelState(Konstante.UNICODE_ESC + "37m");
        }
        #endregion
    }
}