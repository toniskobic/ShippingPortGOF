namespace tskobic_zadaca_3.Memento
{
    public class Memento
    {
        public string Name { get; set; }

        public DateTime VirtualnoVrijeme { get; set; }

        public Memento(string name, DateTime virtualnoVrijeme)
        {
            Name = name;
            VirtualnoVrijeme = virtualnoVrijeme;
        }
    }
}
