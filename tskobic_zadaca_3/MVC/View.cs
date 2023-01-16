using tskobic_zadaca_3.Modeli;
using tskobic_zadaca_3.Observer;
using tskobic_zadaca_3.Singleton;
using tskobic_zadaca_3.Static;

namespace tskobic_zadaca_3.MVC
{
    public class View : IObserver
    {
        public void Update(ISubject s)
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            Terminal terminal = bls.Terminal;
            string state = s.GetState()!;
            if (state!.Length == 1)
            {
                Console.Write(state);
                Thread.Sleep(30);
            }
            else
            {
                if (state.StartsWith("ERROR"))
                {
                    if (!terminal.ZadnjaGreska && bls.BrojGreski != 1)
                    {
                        Console.Write($"{Konstante.UNICODE_ESC}s");
                        Console.Write(Konstante.UNICODE_ESC + $"{terminal.GreskePocetak};{terminal.GreskeKraj}r");
                        Console.Write($"{Konstante.UNICODE_ESC}{terminal.GreskeKraj}d");
                    }
                    terminal.ZadnjaGreska = true;
                    Console.WriteLine(Konstante.UNICODE_ESC + "31m" + state);
                }
                else
                {
                    if (terminal.ZadnjaGreska)
                    {
                        Console.Write(Konstante.UNICODE_ESC + $"{terminal.RadniDioPocetak};{terminal.RadniDioKraj}r");
                        Console.Write($"{Konstante.UNICODE_ESC}u");
                    }
                    terminal.ZadnjaGreska = false;
                    Console.WriteLine(Konstante.UNICODE_ESC + "37m" + state);
                }
                Thread.Sleep(150);
            }
        }
    }
}
