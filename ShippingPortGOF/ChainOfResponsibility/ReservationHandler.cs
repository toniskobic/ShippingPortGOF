using ShippingPortGOF.Singleton;
using ShippingPortGOF.Visitor;

namespace ShippingPortGOF.ChainOfResponsibility
{
    public class ReservationHandler : BaseHandler
    {
        public ReservationHandler() : base()
        {

        }

        public override void Handle(int idShip)
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            DateTime date = sps.VirtualTimeOriginator.VirtualTime;
            TimeOnly time = TimeOnly.FromTimeSpan(date.TimeOfDay);
            DayOfWeek day = date.DayOfWeek;

            Reservation? reservation = sps.ShippingPort!.Reservations.Find(x => x.IdShip == idShip
                        && x.DateFrom.Date.Equals(date.Date) && TimeOnly.FromDateTime(x.DateFrom) <= time
                        && TimeOnly.FromDateTime(x.DateFrom).AddHours(x.HoursDuration) > time);
            if (reservation != null)
            {
                sps.Controller.SetModelState($"Ship {idShip} status: Currently has approved reservation.");
            }
            else
            {
                sps.Controller.SetModelState($"Ship {idShip} status: Currently has no approved reservation.");
                if (NextHandler != null)
                {
                    NextHandler.Handle(idShip);
                }
            }
        }
    }
}
