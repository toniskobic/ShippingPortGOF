using ShippingPortGOF.Models;
using ShippingPortGOF.Observer;
using ShippingPortGOF.Singleton;
using ShippingPortGOF.Static;

namespace ShippingPortGOF.MVC
{
    public class View : IObserver
    {
        public void Update(ISubject s)
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            Terminal terminal = sps.Terminal;
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
                    if (!terminal.LastError && sps.ErrorCount != 1)
                    {
                        Console.Write($"{Constants.UNICODE_ESC}s");
                        Console.Write(Constants.UNICODE_ESC + $"{terminal.ErrorsWindowStartPosition};{terminal.ErrorsWindowEndingPosition}r");
                        Console.Write($"{Constants.UNICODE_ESC}{terminal.ErrorsWindowEndingPosition}d");
                    }
                    terminal.LastError = true;
                    Console.WriteLine(Constants.UNICODE_ESC + "31m" + state);
                }
                else
                {
                    if (terminal.LastError)
                    {
                        Console.Write(Constants.UNICODE_ESC + $"{terminal.OperatingWindowStartPosition};{terminal.OperatingWindowEndPosition}r");
                        Console.Write($"{Constants.UNICODE_ESC}u");
                    }
                    terminal.LastError = false;
                    Console.WriteLine(Constants.UNICODE_ESC + "37m" + state);
                }
                Thread.Sleep(150);
            }
        }
    }
}
