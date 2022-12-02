namespace tskobic_zadaca_2.Modeli
{
    public class VirtualniSat
    {
        public DateTime VirtualnoVrijeme { get; set; }

        public DateTime StvarnoVrijeme { get; set; }

        public void IzvrsiVirtualniPomak()
        {
            DateTime trenutnoVrijeme = DateTime.Now;
            TimeSpan vrijemeProteklo = trenutnoVrijeme.Subtract(StvarnoVrijeme);
            StvarnoVrijeme = trenutnoVrijeme;
            VirtualnoVrijeme = VirtualnoVrijeme.Add(vrijemeProteklo);
        }
    }
}
