using tskobic_zadaca_2.Visitor;

namespace tskobic_zadaca_2.Modeli
{
    public class Rezervacija : IElement
    {
        public int IdVez { get; set; }

        public int IdBrod { get; set; }

        public DateTime DatumOd { get; set; }

        public int SatiTrajanja { get; set; }

        public Rezervacija(int idVez, int idBrod, DateTime datumOd, int satiTrajanja)
        {
            IdVez = idVez;
            IdBrod = idBrod;
            DatumOd = datumOd;
            SatiTrajanja = satiTrajanja;
        }

        public int? Accept(IElementVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
