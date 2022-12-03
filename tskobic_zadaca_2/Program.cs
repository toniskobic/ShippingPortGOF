using System.Text.RegularExpressions;
using tskobic_zadaca_2.FactoryMethod;
using tskobic_zadaca_2.Modeli;
using tskobic_zadaca_2.Singleton;
using tskobic_zadaca_2.Static;

namespace tskobic_zadaca_2
{
    public class Program
    {
        static bool KrajPrograma { get; set; } = false;

        public static void Inicijalizacija(List<Group> grupe)
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();

            Creator creator = new LukaCreator();
            Group? luka = grupe.Find(x => x.Value.StartsWith("-l"));
            string[] podaci = luka!.Value.Split(" ");
            creator.ProcitajPodatke(podaci[1]);
            if (bls.BrodskaLuka == null)
            {
                Ispis.GreskaInicijalizacije();
                KrajPrograma = true;
                return;
            }

            creator = new VezoviCreator();
            Group? vez = grupe.Find(x => x.Value.StartsWith("-v"));
            podaci = vez!.Value.Split(" ");
            creator.ProcitajPodatke(podaci[1]);
            if (bls.BrodskaLuka.Vezovi.Count == 0)
            {
                Ispis.GreskaInicijalizacije();
                KrajPrograma = true;
                return;
            }

            creator = new BrodoviCreator();
            Group? brod = grupe.Find(x => x.Value.StartsWith("-b"));
            podaci = brod!.Value.Split(" ");
            creator.ProcitajPodatke(podaci[1]);
            if (bls.BrodskaLuka.Brodovi.Count == 0)
            {
                Ispis.GreskaInicijalizacije();
                KrajPrograma = true;
                return;
            }

            creator = new RasporediCreator();
            Group? raspored = grupe.Find(x => x.Value.StartsWith("-r"));
            podaci = raspored!.Value.Split(" ");
            creator.ProcitajPodatke(podaci[1]);
        }

        public static void PrivezRezerviranogBroda(string ulaz)
        {
            int id = int.Parse(ulaz);
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            if (!bls.BrodskaLuka.Brodovi.Exists(x => x.ID == id))
            {
                return;
            }
            DateTime datum = bls.VirtualniSat.VirtualnoVrijeme;
            TimeOnly vrijeme = TimeOnly.FromTimeSpan(datum.TimeOfDay);
            DayOfWeek dan = datum.DayOfWeek;
            Privez? postojeciPrivez = bls.BrodskaLuka.Privezi.Find(x => x.IDBrod == id && x.VrijemeOd.Date.Equals(datum.Date)
                && TimeOnly.FromTimeSpan(x.VrijemeOd.TimeOfDay) <= vrijeme
                && TimeOnly.FromTimeSpan(x.VrijemeDo.TimeOfDay) > vrijeme);
            if (postojeciPrivez == null)
            {
                Raspored? raspored = bls.BrodskaLuka.Rasporedi.Find(x => x.IDBrod == id
                    && x.DaniUTjednu.Contains(dan) && x.VrijemeOd <= vrijeme && x.VrijemeDo > vrijeme);
                if (raspored != null)
                {
                    Privez privez = new Privez(raspored.IDVez, id, datum, datum.Date.Add(raspored.VrijemeDo.ToTimeSpan()));
                    bls.BrodskaLuka.Privezi.Add(privez);
                }
                else
                {
                    Rezervacija? rezervacija = bls.BrodskaLuka.Rezervacije.Find(x => x.IDBrod == id
                        && x.DatumOd.Date.Equals(datum.Date) && TimeOnly.FromDateTime(x.DatumOd) <= vrijeme
                        && TimeOnly.FromDateTime(x.DatumOd).AddHours(x.SatiTrajanja) > vrijeme);
                    if (rezervacija != null)
                    {
                        Privez privez = new Privez(rezervacija.IDVez, id,
                            datum, rezervacija.DatumOd.AddHours(rezervacija.SatiTrajanja));
                        bls.BrodskaLuka.Privezi.Add(privez);
                    }
                }
            }
        }

        private static void PrivezSlobodnogBroda(string idBroda, string trajanje)
        {
            int id = int.Parse(idBroda);
            int sati = int.Parse(trajanje);
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            Brod? brod = bls.BrodskaLuka.Brodovi.Find(x => x.ID == id);
            if (brod == null)
            {
                return;
            }
            DateTime datum = bls.VirtualniSat.VirtualnoVrijeme;
            TimeOnly vrijeme = TimeOnly.FromTimeSpan(datum.TimeOfDay);
            DayOfWeek dan = datum.DayOfWeek;
            Privez? postojeciPrivez = bls.BrodskaLuka.Privezi.Find(x => x.IDBrod == id && x.VrijemeOd.Date.Equals(datum.Date)
                && TimeOnly.FromTimeSpan(x.VrijemeOd.TimeOfDay) <= vrijeme
                && TimeOnly.FromTimeSpan(x.VrijemeDo.TimeOfDay) > vrijeme);
            if (postojeciPrivez == null)
            {
                List<Vez> vezovi = Utils.PronadjiVezove(brod);
                if (vezovi.Count > 0)
                {
                    List<Vez> fVezoviPrivezi = vezovi.FindAll(vez => bls.BrodskaLuka.Privezi.Any(x => x.IDVez == vez.ID
                        && x.VrijemeOd <= datum.AddHours(sati) && datum <= x.VrijemeDo));

                    List<Vez> fVezoviRasporedi = vezovi.FindAll(vez => bls.BrodskaLuka.Rasporedi.Any(x => x.IDVez == vez.ID
                        && x.DaniUTjednu.Contains(dan) && x.VrijemeOd <= vrijeme.AddHours(sati) && vrijeme <= x.VrijemeDo));

                    List<Vez> fVezoviRezervacije = vezovi.FindAll(vez => bls.BrodskaLuka.Rezervacije.Any(x => x.IDVez == vez.ID
                        && x.DatumOd.Date == datum.Date
                        && TimeOnly.FromTimeSpan(x.DatumOd.TimeOfDay) <= vrijeme.AddHours(sati)
                        && vrijeme <= TimeOnly.FromTimeSpan(x.DatumOd.TimeOfDay).AddHours(x.SatiTrajanja)));

                    Vez? vez = vezovi.Except(fVezoviPrivezi)
                        .Except(fVezoviRasporedi).Except(fVezoviRezervacije).ToList().FirstOrDefault();
                    if (vez != null)
                    {
                        Privez privez = new Privez(vez.ID, id, datum, datum.AddHours(sati));
                        bls.BrodskaLuka.Privezi.Add(privez);
                    }
                }
            }
        }

        private static void IspisStatusaVezova()
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            List<Vez> vezovi = bls.BrodskaLuka.Vezovi;

            DateTime datum = bls.VirtualniSat.VirtualnoVrijeme;
            TimeOnly vrijeme = TimeOnly.FromTimeSpan(datum.TimeOfDay);
            DayOfWeek dan = datum.DayOfWeek;

            List<Vez> fVezoviPrivezi = vezovi.FindAll(vez => bls.BrodskaLuka.Privezi.Any(x => x.IDVez == vez.ID
                && x.VrijemeOd <= datum && datum <= x.VrijemeDo));

            List<Vez> fVezoviRasporedi = vezovi.FindAll(vez => bls.BrodskaLuka.Rasporedi.Any(x => x.IDVez == vez.ID
                 && x.DaniUTjednu.Contains(dan) && x.VrijemeOd <= vrijeme && vrijeme <= x.VrijemeDo));

            List<Vez> fVezoviRezervacije = vezovi.FindAll(vez => bls.BrodskaLuka.Rezervacije.Any(x => x.IDVez == vez.ID
                && x.DatumOd.Date == datum.Date
                && TimeOnly.FromTimeSpan(x.DatumOd.TimeOfDay) <= vrijeme
                && vrijeme <= TimeOnly.FromTimeSpan(x.DatumOd.TimeOfDay).AddHours(x.SatiTrajanja)));

            List<Vez> zauzetiVezovi = fVezoviPrivezi.Union(fVezoviRasporedi).Union(fVezoviRezervacije).Distinct().ToList();

            Ispis.Zaglavlje("Status vezova");
            foreach (Vez vez in vezovi.Except(zauzetiVezovi).ToList())
            {
                Ispis.Vez(vez.ID.ToString(), vez.Oznaka, vez.Vrsta, "Slobodan");
            }
            foreach (Vez vez in zauzetiVezovi)
            {
                Ispis.Vez(vez.ID.ToString(), vez.Oznaka, vez.Vrsta, "Zauzet");

            }
        }

        //TODO Implementacija V naredbe
        public static void IspisVezovaVrste(string vrsta, string status, string datumOd, string datumDo)
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            List<Vez> vezovi = bls.BrodskaLuka.Vezovi;

            Utils.ProvjeriPretvorbuUDatum(datumOd, out DateTime intervalOd);
            Utils.ProvjeriPretvorbuUDatum(datumDo, out DateTime intervalDo);
        }

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

                Inicijalizacija(grupe);
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
                                PrivezRezerviranogBroda(ulaz.Substring(3));
                                break;
                            }
                        case string ulaz when new Regex(Konstante.ZahtjevSlobPriveza).IsMatch(ulaz):
                            {
                                bls.VirtualniSat.IzvrsiVirtualniPomak();
                                Ispis.VirtualniSat();
                                string[] podaci = ulaz.Split(" ");
                                PrivezSlobodnogBroda(podaci[1], podaci[2]);
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
    }
}