using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using tskobic_zadaca_1.Citaci;
using tskobic_zadaca_1.FactoryMethod;
using tskobic_zadaca_1.Singleton;

namespace tskobic_zadaca_1
{
    public class Program
    {
        static bool KrajPrograma { get; set; } = false;

        static void Main(string[] args)
        {
            String regex = @"^(?:(-r [a-zA-Z_0-9.]+\.csv)(?!.*\1) )?(-[lvb] [a-zA-Z_0-9.]+\.csv)(?!.*\2)"
            + @"(?: (-r [a-zA-Z_0-9.]+\.csv)(?!.*\3))?"
            + @"(?: (-[lvb] [a-zA-Z_0-9.]+\.csv)(?!.*\4))(?: (-r [a-zA-Z_0-9.]+\.csv)(?!.*\5))?"
            + @"(?: (-[lvb] [a-zA-Z_0-9.]+\.csv)(?!.*\6))(?: (-r [a-zA-Z_0-9.]+\.csv)(?!.*\7))?$";
            Regex re = new Regex(regex);

            Regex rg = new Regex(regex);
            Match match = rg.Match(string.Join(" ", args));
            List<Group> grupe = new List<Group>();

            if (match.Success)
            {
                BrodskaLukaSingleton brodskaLukaSingleton = BrodskaLukaSingleton.Instanca();
                for (int i = 1; i < match.Groups.Count; i++)
                {
                    if (match.Groups[i].Success)
                    {
                        grupe.Add(match.Groups[i]);
                    }
                }

                Group luka = grupe.Find(x => x.Value.StartsWith("-l"));
                string[] podaci = luka.Value.Split(" ");
                CitacLuke citacLuke = new CitacLuke();
                citacLuke.ProcitajPodatke(podaci[1]);
                Group vez = grupe.Find(x => x.Value.StartsWith("-v"));
                podaci = vez.Value.Split(" ");
                CitacVezova citacVezova = new CitacVezova();
                brodskaLukaSingleton.BrodskaLuka.Vezovi.AddRange(citacVezova.ProcitajPodatke(podaci[1]));
                Group brod = grupe.Find(x => x.Value.StartsWith("-b"));
                podaci = brod.Value.Split(" ");
                CitacBrodova citacBrodova = new CitacBrodova();
                brodskaLukaSingleton.BrodskaLuka.Brodovi.AddRange(citacBrodova.ProcitajPodatke(podaci[1]));

                while (!KrajPrograma)
                {
                    Console.WriteLine("\nUnesite komandu");
                    string komanda = Console.ReadLine();
                    switch (komanda)
                    {
                        case "Q":
                            {
                                Program.KrajPrograma = true;
                                break;
                            }
                        default:
                            break;
                    }
                }

            }
            else
            {
                Console.WriteLine("ERROR: Neispravno uneseni argumenti!");
            }
        }
    }
}