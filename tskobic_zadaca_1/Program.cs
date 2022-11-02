using System.Text.RegularExpressions;
using tskobic_zadaca_1.Citaci;
using tskobic_zadaca_1.FactoryMethod;
using tskobic_zadaca_1.Singleton;
using tskobic_zadaca_1.Static;

namespace tskobic_zadaca_1
{
    public class Program
    {
        static bool KrajPrograma { get; set; } = false;

        static string UlazniArgumentiIzraz { get; } = @"^(?:(-r [a-zA-Z_0-9.]+\.csv)(?!.*\1) )?(-[lvb] [a-zA-Z_0-9.]+\.csv)(?!.*\2)"
            + @"(?: (-r [a-zA-Z_0-9.]+\.csv)(?!.*\3))?"
            + @"(?: (-[lvb] [a-zA-Z_0-9.]+\.csv)(?!.*\4))(?: (-r [a-zA-Z_0-9.]+\.csv)(?!.*\5))?"
            + @"(?: (-[lvb] [a-zA-Z_0-9.]+\.csv)(?!.*\6))(?: (-r [a-zA-Z_0-9.]+\.csv)(?!.*\7))?$";

        static string VirtualnoVrijemeIzraz { get; } = @"^VR ([1-9]|([012][0-9])|(3[01]))."
            + @"([0]{0,1}[1-9]|1[012]).\d\d\d\d. ([0-1]?[0-9]|2?[0-3]):([0-5]\d):([0-5]\d)$";

        public static void Inicijalizacija(List<Group> grupe)
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();

            Group luka = grupe.Find(x => x.Value.StartsWith("-l"));
            string[] podaci = luka.Value.Split(" ");
            CitacLuke citacLuke = new CitacLuke();
            citacLuke.ProcitajPodatke(podaci[1]);

            Group vez = grupe.Find(x => x.Value.StartsWith("-v"));
            podaci = vez.Value.Split(" ");
            CitacVezova citacVezova = new CitacVezova();
            bls.BrodskaLuka.Vezovi.AddRange(citacVezova.ProcitajPodatke(podaci[1]));

            Group brod = grupe.Find(x => x.Value.StartsWith("-b"));
            podaci = brod.Value.Split(" ");
            CitacBrodova citacBrodova = new CitacBrodova();
            bls.BrodskaLuka.Brodovi.AddRange(citacBrodova.ProcitajPodatke(podaci[1]));

            Group raspored = grupe.Find(x => x.Value.StartsWith("-r"));
            podaci = raspored.Value.Split(" ");
            CitacRasporeda citacRasporeda = new CitacRasporeda();
            citacRasporeda.ProcitajPodatke(podaci[1]);
        }

        static void Main(string[] args)
        {
            Regex rg = new Regex(UlazniArgumentiIzraz);
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
                        case "Q":
                            {
                                bls.VirtualniSat.IzvrsiVirtualniPomak();
                                Ispis.VirtualniSat();
                                KrajPrograma = true;
                                break;
                            }
                        case string ulaz when new Regex(VirtualnoVrijemeIzraz).IsMatch(ulaz):
                            {
                                bls.VirtualniSat.StvarnoVrijeme = DateTime.Now;
                                bls.VirtualniSat.VirtualnoVrijeme = DateTime.Parse(ulaz.Substring(3));
                                Ispis.VirtualniSat();
                                break;
                            }
                        default:
                            {
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