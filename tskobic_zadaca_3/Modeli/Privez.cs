using tskobic_zadaca_3.Visitor;

namespace tskobic_zadaca_3.Modeli
{
    public class Privez : IElement
    {
        public int IdVez { get; set; }

        public int IdBrod { get; set; }

        public DateTime VrijemeOd { get; set; }

        public DateTime VrijemeDo { get; set; }

        public Privez(int idVez, int idBrod, DateTime vrijemeOd, DateTime vrijemeDo)
        {
            IdVez = idVez;
            IdBrod = idBrod;
            VrijemeOd = vrijemeOd;
            VrijemeDo = vrijemeDo;
        }

        public int? Accept(IElementVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
