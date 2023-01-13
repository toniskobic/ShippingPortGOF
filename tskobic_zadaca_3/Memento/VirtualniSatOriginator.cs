using tskobic_zadaca_3.Memento;

namespace tskobic_zadaca_3.Modeli
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

        public void GetStateFromMemento(Memento.Memento m)
        {
            VirtualnoVrijeme = m.VirtualnoVrijeme;
        }

        public Memento.Memento SaveStateToMemento(string naziv)
        {
            return new Memento.Memento(naziv, VirtualnoVrijeme);
        }

    }
}
