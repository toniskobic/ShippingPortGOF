namespace ShippingPortGOF.Visitor
{
    public class Schedule : IElement
    {
        public int IdMooring { get; set; }

        public int IdShip { get; set; }

        public List<DayOfWeek> DaysOfWeek { get; set; }

        public TimeOnly TimeFrom { get; set; }

        public TimeOnly TimeTo { get; set; }

        public Schedule(int idMooring, int idShip, List<DayOfWeek> DaysOfWeek, TimeOnly timeFrom, TimeOnly timeTo)
        {
            IdMooring = idMooring;
            IdShip = idShip;
            this.DaysOfWeek = DaysOfWeek;
            TimeFrom = timeFrom;
            TimeTo = timeTo;
        }

        public int? Accept(IElementVisitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}
