using ShippingPortGOF.Composite;
using ShippingPortGOF.Memento;
using ShippingPortGOF.Models;
using ShippingPortGOF.MVC;

namespace ShippingPortGOF.Singleton
{
    public class ShippingPortSingleton
    {
        private static ShippingPortSingleton? instance;

        public ShippingPortComposite? ShippingPort { get; set; }

        public VirtualTimeOriginator VirtualTimeOriginator { get; set; }

        public Controller Controller { get; set; }

        public CareTaker CareTaker { get; set; }

        public int ErrorCount { get; set; }

        public bool HeaderPrint { get; set; }

        public bool FooterPrint { get; set; }

        public bool SequenceNumberPrint { get; set; }

        private ShippingPortSingleton()
        {
            VirtualTimeOriginator = new VirtualTimeOriginator();
            Controller = new Controller();
            CareTaker = new CareTaker();
            ErrorCount = 0;
            HeaderPrint = false;
            FooterPrint = false;
            SequenceNumberPrint = false;
        }

        public static ShippingPortSingleton GetInstance()
        {
            if (instance == null)
            {
                instance = new ShippingPortSingleton();
            }

            return instance;
        }
    }
}
