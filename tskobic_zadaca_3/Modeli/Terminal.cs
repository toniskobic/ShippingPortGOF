namespace tskobic_zadaca_3.Modeli
{
    public class Terminal
    {
        public int BrojRedaka { get; set; }

        public string? OmjerEkrana { get; set; }

        public bool RadniDioPrvi { get; set; }

        public int RadniDioPocetak { get; set; }

        public int RadniDioKraj { get; set; }

        public int GreskePocetak { get; set; }

        public int GreskeKraj { get; set; }

        public int Sredina { get; set; }

        public bool ZadnjaGreska { get; set; } = false;
    }
}
