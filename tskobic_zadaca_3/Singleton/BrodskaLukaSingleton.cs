﻿using tskobic_zadaca_3.Composite;
using tskobic_zadaca_3.Memento;
using tskobic_zadaca_3.Modeli;
using tskobic_zadaca_3.MVC;

namespace tskobic_zadaca_3.Singleton
{
    public class BrodskaLukaSingleton
    {
        private static BrodskaLukaSingleton? instanca;

        public BrodskaLukaComposite? BrodskaLuka { get; set; }

        public VirtualniSatOriginator VirtualniSatOriginator { get; set; }

        public Terminal Terminal { get; set; }

        public Controller Controller { get; set; }

        public CareTaker CareTaker { get; set; }

        public int BrojGreski { get; set; }

        public bool Zaglavlje { get; set; }

        public bool Podnozje { get; set; }

        public bool RedniBroj { get; set; }

        private BrodskaLukaSingleton()
        {
            VirtualniSatOriginator = new VirtualniSatOriginator();
            Terminal = new Terminal();
            Controller = new Controller();
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
