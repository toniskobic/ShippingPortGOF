namespace ShippingPortGOF.Visitor
{
    public class MooredShip : IElement
    {
        public int IdMooring { get; set; }

        public int IdShip { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public MooredShip(int idMooring, int idShip, DateTime dateFrom, DateTime dateTo)
        {
            IdMooring = idMooring;
            IdShip = idShip;
            DateFrom = dateFrom;
            DateTo = dateTo;
        }

        public int? Accept(IElementVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
