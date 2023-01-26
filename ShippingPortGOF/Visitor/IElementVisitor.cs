namespace ShippingPortGOF.Visitor
{
    public interface IElementVisitor
    {
        public int? Visit(MooredShip mooredShip);

        public int? Visit(Schedule schedule);

        public int? Visit(Reservation reservation);
    }
}
