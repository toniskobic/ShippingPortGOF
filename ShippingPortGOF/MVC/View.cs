using ShippingPortGOF.Observer;

namespace ShippingPortGOF.MVC
{
    public class View : IObserver
    {
        public ConsoleColor ForegroundColor
        {
            get
            {
                return Console.ForegroundColor;
            }
            set
            {
                Console.ForegroundColor = value;
            }
        }

        public void Update(ISubject s)
        {
            string state = s.GetState()!;
            if (state.StartsWith("ERROR"))
            {
                ForegroundColor = ConsoleColor.Red;
            }
            WriteLine(state);
            if (ForegroundColor != ConsoleColor.White)
            {
                ForegroundColor = ConsoleColor.White;
            }
            Thread.Sleep(30);
        }

        private void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
    }
}
