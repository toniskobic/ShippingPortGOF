using ShippingPortGOF.Singleton;
using ShippingPortGOF.Visitor;

namespace ShippingPortGOF.ChainOfResponsibility
{
    public class ScheduleHandler : BaseHandler
    {
        public ScheduleHandler() : base()
        {

        }

        public override void Handle(int idShip)
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            DateTime date = sps.VirtualTimeOriginator.VirtualTime;
            TimeOnly time = TimeOnly.FromTimeSpan(date.TimeOfDay);
            DayOfWeek day = date.DayOfWeek;

            Schedule? schedule = sps.ShippingPort!.Schedules.Find(x => x.IdShip == idShip
                && x.DaysOfWeek.Contains(day) && x.TimeFrom <= time && x.TimeTo > time);
            if (schedule != null)
            {
                sps.Controller.SetModelState($"Ship {idShip} status: Currently has a scheduled reservation");
            } else
            {
                sps.Controller.SetModelState($"Ship {idShip} status: Currently does not have a scheduled reservation.");
                if (NextHandler != null)
                {
                    NextHandler.Handle(idShip);
                }
            }
        }
    }
}
