namespace ShippingPortGOF.Visitor
{
    public class Reservation : IElement
    {
        public int IdMooring { get; set; }

        public int IdShip { get; set; }

        public DateTime DateFrom { get; set; }

        public int HoursDuration { get; set; }

        public Reservation(int idMooring, int idShip, DateTime dateFrom, int hoursDuration)
        {
            IdMooring = idMooring;
            IdShip = idShip;
            DateFrom = dateFrom;
            HoursDuration = hoursDuration;
        }

        public int? Accept(IElementVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
