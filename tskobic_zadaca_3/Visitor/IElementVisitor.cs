using tskobic_zadaca_3.Modeli;

namespace tskobic_zadaca_3.Visitor
{
    public interface IElementVisitor
    {
        public int? Visit(Privez privez);

        public int? Visit(Raspored raspored);

        public int? Visit(Rezervacija rezervacija);
    }
}
