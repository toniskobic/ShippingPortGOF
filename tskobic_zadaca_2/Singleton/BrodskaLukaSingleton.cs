﻿using tskobic_zadaca_2.Modeli;

namespace tskobic_zadaca_2.Singleton
{
    public class BrodskaLukaSingleton
    {
        private static BrodskaLukaSingleton instanca;

        public BrodskaLuka BrodskaLuka { get; set; }

        public VirtualniSat VirtualniSat { get; set; }

        public int BrojGreski { get; set; }

        private BrodskaLukaSingleton()
        {
            VirtualniSat = new VirtualniSat();
            BrojGreski = 0;
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