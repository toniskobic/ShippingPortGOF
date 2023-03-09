using ShippingPortGOF.Observer;
using ShippingPortGOF.Singleton;

namespace ShippingPortGOF.MVC
{
    public class View : IObserver
    {
        public void Update(ISubject s)
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            string state = s.GetState()!;
            if (state.StartsWith("ERROR"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine(state);
            Thread.Sleep(30);
        }
    }
}
