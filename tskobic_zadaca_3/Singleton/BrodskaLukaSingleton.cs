using tskobic_zadaca_3.Memento;
using tskobic_zadaca_3.Modeli;

namespace tskobic_zadaca_3.Singleton
{
    public class BrodskaLukaSingleton
    {
        private static BrodskaLukaSingleton? instanca;

        public BrodskaLuka? BrodskaLuka { get; set; }

        public VirtualniSatOriginator VirtualniSatOriginator { get; set; }

        public CareTaker CareTaker { get; set; }

        public int BrojGreski { get; set; }

        public bool Zaglavlje { get; set; }

        public bool Podnozje { get; set; }

        public bool RedniBroj { get; set; }

        private BrodskaLukaSingleton()
        {
            VirtualniSatOriginator = new VirtualniSatOriginator();
            CareTaker = new CareTaker();
            BrojGreski = 0;
            Zaglavlje = false;
            Podnozje = false;
            RedniBroj = false;
        }

        public static BrodskaLukaSingleton Instanca()
        {
            if (instanca == null)
            {
                instanca = new BrodskaLukaSingleton();
            }

            return instanca;
        }
    }
}
