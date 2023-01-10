using tskobic_zadaca_3.Visitor;

namespace tskobic_zadaca_3.Modeli
{
    public class Raspored : IElement
    {
        public int IdVez { get; set; }

        public int IdBrod { get; set; }

        public List<DayOfWeek> DaniUTjednu { get; set; }

        public TimeOnly VrijemeOd { get; set; }

        public TimeOnly VrijemeDo { get; set; }

        public Raspored(int idVez, int idBrod, List<DayOfWeek> daniUTjednu, TimeOnly vrijemeOd, TimeOnly vrijemeDo)
        {
            IdVez = idVez;
            IdBrod = idBrod;
            DaniUTjednu = daniUTjednu;
            VrijemeOd = vrijemeOd;
            VrijemeDo = vrijemeDo;
        }

        public int? Accept(IElementVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
