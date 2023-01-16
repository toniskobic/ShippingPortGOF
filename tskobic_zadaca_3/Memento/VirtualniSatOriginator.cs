namespace tskobic_zadaca_3.Memento
{
    public class VirtualniSatOriginator
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

        public void GetStateFromMemento(Memento m)
        {
            VirtualnoVrijeme = m.VirtualnoVrijeme;
        }

        public Memento SaveStateToMemento(string naziv)
        {
            return new Memento(naziv, VirtualnoVrijeme);
        }
    }
}
