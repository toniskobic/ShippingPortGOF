using System.Globalization;
using System.Text.RegularExpressions;
using tskobic_zadaca_2.ChainOfResponsibility;
using tskobic_zadaca_2.FactoryMethod;
using tskobic_zadaca_2.Modeli;
using tskobic_zadaca_2.Singleton;
using tskobic_zadaca_2.Static;
using tskobic_zadaca_2.Visitor;

namespace tskobic_zadaca_2
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
            if (bls.BrodskaLuka.Molovi.Count == 0)
            {
                return false;
            }

            UcitajPodatke(new VezoviCreator(), grupe, "-v");
            if (bls.BrodskaLuka.Vezovi.Count == 0)
            {
                return false;
            }

            UcitajPodatke(new MoloviVezoviCreator(), grupe, "-mv");
            if (!bls.BrodskaLuka.Vezovi.Exists(x => x.IdMol != null))
            {
                return false;
            }
            bls.BrodskaLuka.Vezovi.RemoveAll(x => x.IdMol == null);

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

        private static void PrivezRezerviranogBroda(int idBrod)
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            Brod? brod = bls.BrodskaLuka!.Brodovi.Find(x => x.ID == idBrod);
            if (brod == null)
            {
                Console.WriteLine($"Brod s proslijeđenim ID-em {idBrod} ne postoji.");
                return;
            }
            List<Kanal> kanali = bls.BrodskaLuka!.Kanali;
            Kanal? kanal = kanali.Find(x => x.Observers.Contains(brod));
            if (kanal == null)
            {
                Console.WriteLine($"Brod s ID-em {idBrod} mora biti u komunikaciji "
                    + $"s kapetanijom da bi zatražio privez.");
                return;
            }
            DateTime datum = bls.VirtualniSat.VirtualnoVrijeme;
            TimeOnly vrijeme = TimeOnly.FromTimeSpan(datum.TimeOfDay);
            DayOfWeek dan = datum.DayOfWeek;
            Privez? postojeciPrivez = bls.BrodskaLuka.Privezi.Find(x => x.IdBrod == idBrod
                && x.VrijemeOd.Date.Equals(datum.Date)
                && TimeOnly.FromTimeSpan(x.VrijemeOd.TimeOfDay) <= vrijeme
                && TimeOnly.FromTimeSpan(x.VrijemeDo.TimeOfDay) > vrijeme);
            string poruka = "";
            if (postojeciPrivez == null)
            {
                Raspored? raspored = bls.BrodskaLuka.Rasporedi.Find(x => x.IdBrod == idBrod
                    && x.DaniUTjednu.Contains(dan) && x.VrijemeOd <= vrijeme && x.VrijemeDo > vrijeme);
                if (raspored != null)
                {
                    DateTime vrijemeDo = datum.Date.Add(raspored.VrijemeDo.ToTimeSpan());
                    Privez privez = new Privez(raspored.IdVez, idBrod, datum, vrijemeDo);
                    bls.BrodskaLuka.Privezi.Add(privez);
                    bls.BrodskaLuka.Dnevnik.Add(new Zapis(VrstaZahtjeva.ZD, idBrod, false, datum, vrijemeDo));
                    ZapisiPoruku(kanal, $"Brodu s ID-em {brod.ID} odobren privez na vez {raspored.IdVez}.");
                    Console.WriteLine("Naredba uspješna.");
                    return;
                }
                else
                {
                    Rezervacija? rezervacija = bls.BrodskaLuka.Rezervacije.Find(x => x.IdBrod == idBrod
                        && x.DatumOd.Date.Equals(datum.Date) && TimeOnly.FromDateTime(x.DatumOd) <= vrijeme
                        && TimeOnly.FromDateTime(x.DatumOd).AddHours(x.SatiTrajanja) > vrijeme);
                    if (rezervacija != null)
                    {
                        DateTime vrijemeDo = rezervacija.DatumOd.AddHours(rezervacija.SatiTrajanja);
                        Privez privez = new Privez(rezervacija.IdVez, idBrod,
                            datum, vrijemeDo);
                        bls.BrodskaLuka.Privezi.Add(privez);
                        bls.BrodskaLuka.Dnevnik.Add(new Zapis(VrstaZahtjeva.ZD, idBrod, false, datum, vrijemeDo));
                        ZapisiPoruku(kanal, $"Brodu s ID-em {brod.ID} odobren privez na vez {rezervacija.IdVez}.");
                        Console.WriteLine("Naredba uspješna.");
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
            ZapisiPoruku(kanal, $"{kanal.Frekvencija}: {poruka}");
            Console.WriteLine(poruka);
        }

        private static void PrivezSlobodnogBroda(int idBrod, int trajanje)
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            Brod? brod = bls.BrodskaLuka!.Brodovi.Find(x => x.ID == idBrod);
            if (brod == null)
            {
                Console.WriteLine($"Brod s proslijeđenim ID-em {idBrod} ne postoji.");
                return;
            }
            List<Kanal> kanali = bls.BrodskaLuka!.Kanali;
            Kanal? kanal = kanali.Find(x => x.Observers.Contains(brod));
            if (kanal == null)
            {
                Console.WriteLine($"Brod s ID-em {idBrod} mora biti u komunikaciji "
                    + $"s kapetanijom da bi zatražio privez.");
                return;
            }
            DateTime datum = bls.VirtualniSat.VirtualnoVrijeme;
            TimeOnly vrijeme = TimeOnly.FromTimeSpan(datum.TimeOfDay);
            DayOfWeek dan = datum.DayOfWeek;
            Privez? postojeciPrivez = bls.BrodskaLuka.Privezi.Find(x => x.IdBrod == idBrod
                && x.VrijemeOd.Date.Equals(datum.Date)
                && TimeOnly.FromTimeSpan(x.VrijemeOd.TimeOfDay) <= vrijeme
                && TimeOnly.FromTimeSpan(x.VrijemeDo.TimeOfDay) > vrijeme);
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
                        Console.WriteLine("Naredba uspješna.");
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
            ZapisiPoruku(kanal, $"{kanal.Frekvencija}: {poruka}");
            Console.WriteLine(poruka);
        }

        private static void IspisStatusaVezova()
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            List<Vez> vezovi = bls.BrodskaLuka!.Vezovi;

            DateTime datum = bls.VirtualniSat.VirtualnoVrijeme;
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
            List<Vez> vezovi = bls.BrodskaLuka!.Vezovi;

            Utils.ProvjeriPretvorbuUDatum(datumOd, out DateTime intervalOd);
            Utils.ProvjeriPretvorbuUDatum(datumDo, out DateTime intervalDo);
        }

        private static void IspisZauzetihVezova(string ulaz)
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            List<Vez> vezovi = bls.BrodskaLuka!.Vezovi;

            DateTime.TryParseExact(ulaz, "dd.MM.yyyy. HH:mm", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out DateTime datum);
            TimeOnly vrijeme = TimeOnly.FromTimeSpan(datum.TimeOfDay);
            DayOfWeek dan = datum.DayOfWeek;

            List<Privez> privezi = bls.BrodskaLuka!.Privezi;
            List<Raspored> rasporedi = bls.BrodskaLuka!.Rasporedi;
            List<Rezervacija> rezervacije = bls.BrodskaLuka!.Rezervacije;

            List<IElement> elementi = privezi.ToList<IElement>().Concat(rasporedi).Concat(rezervacije).ToList();

            List<Vez> zauzetiVezovi = new List<Vez>();
            IElementVisitor elementVisitor = new ElementVisitor(datum);

            foreach (IElement element in elementi)
            {
                int? id = element.Accept(elementVisitor);
                if (id != null)
                {
                    Vez? vez = vezovi.Find(x => x.ID == id);
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
            Console.WriteLine("Svi vezovi su slobodni.");
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
                Console.WriteLine("Naredbna neuspješna, uneseni kanal ne postoji.");
                return;
            }
            Brod? brod = bls.BrodskaLuka!.Brodovi.Find(x => x.ID == idBrod);
            if (brod == null)
            {
                Console.WriteLine("Naredba neuspješna, brod s proslijeđenim ID-em ne postoji.");
                return;
            }
            if (!odjava)
            {
                if (kanal.Zauzet())
                {
                    Console.WriteLine("Naredba neuspješna, maksimalan broj komunikacija na kanalu.");
                    return;
                }
                if (kanali.Exists(x => x.Observers.Contains(brod)))
                {
                    Console.WriteLine("Naredba neuspješna, brod već ima aktivnu komunikaciju");
                    return;
                }
                kanal.Attach(brod);
                Console.WriteLine("Naredba uspješna");
            }
            else if (odjava)
            {
                if (kanal.Observers.Contains(brod))
                {
                    DateTime vrijeme = bls.VirtualniSat.VirtualnoVrijeme;
                    Privez? privez = bls.BrodskaLuka!.Privezi.Find(x => x.IdBrod == brod.ID
                        && x.VrijemeOd <= vrijeme && vrijeme <= x.VrijemeDo);
                    if (privez != null)
                    {
                        privez.VrijemeDo = vrijeme;
                    }
                    kanal.Detach(brod);
                    Console.WriteLine($"Naredba uspješna, brod {idBrod} je odjavljen sa kanal {frekvencija}.");
                }
                else
                {
                    Console.WriteLine($"Naredba neuspješna, ne postoji aktivna komunikacija "
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
            List<Kanal> kanali = bls.BrodskaLuka!.Kanali;
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
                Console.WriteLine($"Brod s proslijeđenim ID-em {idBrod} ne postoji.");
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
            Regex rg = new Regex(Konstante.UlazniArgumenti);
            Match match = rg.Match(string.Join(" ", args));
            List<Group> grupe = new List<Group>();

            if (match.Success)
            {
                BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
                for (int i = 1; i < match.Groups.Count; i++)
                {
                    if (match.Groups[i].Success)
                    {
                        grupe.Add(match.Groups[i]);
                    }
                }

                if (!Inicijalizacija(grupe))
                {
                    Ispis.GreskaInicijalizacije();
                    return;
                }
                bls.VirtualniSat.StvarnoVrijeme = DateTime.Now;

                while (!KrajPrograma)
                {
                    Console.WriteLine("\nUnesite komandu:");
                    switch (Console.ReadLine())
                    {
                        case "I":
                            {
                                bls.VirtualniSat.IzvrsiVirtualniPomak();
                                Ispis.VirtualniSat();
                                IspisStatusaVezova();
                                break;
                            }
                        case string ulaz when new Regex(Konstante.IspisVezova).IsMatch(ulaz):
                            {
                                bls.VirtualniSat.IzvrsiVirtualniPomak();
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
                                bls.VirtualniSat.StvarnoVrijeme = DateTime.Now;
                                bls.VirtualniSat.VirtualnoVrijeme = DateTime.Parse(ulaz.Substring(3));
                                Ispis.VirtualniSat();
                                break;
                            }
                        case string ulaz when new Regex(Konstante.ZahtjevRezervacije).IsMatch(ulaz):
                            {
                                Creator creator = new RezervacijeCreator();
                                bls.VirtualniSat.IzvrsiVirtualniPomak();
                                Ispis.VirtualniSat();
                                creator.ProcitajPodatke(ulaz.Substring(3));
                                break;
                            }
                        case string ulaz when new Regex(Konstante.ZahtjevRezPriveza).IsMatch(ulaz):
                            {
                                bls.VirtualniSat.IzvrsiVirtualniPomak();
                                Ispis.VirtualniSat();
                                int idBrod = int.Parse(ulaz.Substring(3));
                                ZapisiPoruku(idBrod, ulaz);
                                PrivezRezerviranogBroda(idBrod);
                                break;
                            }
                        case string ulaz when new Regex(Konstante.ZahtjevSlobPriveza).IsMatch(ulaz):
                            {
                                bls.VirtualniSat.IzvrsiVirtualniPomak();
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
                                bls.VirtualniSat.IzvrsiVirtualniPomak();
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
                                bls.VirtualniSat.IzvrsiVirtualniPomak();
                                Ispis.VirtualniSat();
                                UredjenjeIspisa(ulaz);
                                break;
                            }
                        case string ulaz when new Regex(Konstante.IspisZauzetihVezova).IsMatch(ulaz):
                            {
                                bls.VirtualniSat.IzvrsiVirtualniPomak();
                                Ispis.VirtualniSat();
                                IspisZauzetihVezova(ulaz.Substring(3));
                                break;
                            }
                        case string ulaz when new Regex(Konstante.StatusBroda).IsMatch(ulaz):
                            {
                                bls.VirtualniSat.IzvrsiVirtualniPomak();
                                Ispis.VirtualniSat();
                                string[] podaci = ulaz.Split(" ");
                                int idBrod = int.Parse(podaci[1]);
                                StatusBroda(idBrod);
                                break;
                            }
                        case "Q":
                            {
                                bls.VirtualniSat.IzvrsiVirtualniPomak();
                                Ispis.VirtualniSat();
                                KrajPrograma = true;
                                break;
                            }
                        default:
                            {
                                bls.VirtualniSat.IzvrsiVirtualniPomak();
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
        }
        #endregion
    }
}