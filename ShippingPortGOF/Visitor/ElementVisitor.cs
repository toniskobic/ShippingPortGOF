namespace ShippingPortGOF.Visitor
{
    public class ElementVisitor : IElementVisitor
    {
        public DateTime Date { get; set; }

        public ElementVisitor(DateTime date)
        {
            Date = date;
        }

        public int? Visit(MooredShip mooredShip)
        {
            if (mooredShip.DateFrom <= Date.AddSeconds(60) && Date <= mooredShip.DateTo)
            {
                return mooredShip.IdMooring;
            }
            return null;
        }

        public int? Visit(Schedule schedule)
        {
            TimeOnly time = TimeOnly.FromTimeSpan(Date.TimeOfDay);
            DayOfWeek day = Date.DayOfWeek;

            if (schedule.DaysOfWeek.Contains(day) && schedule.TimeFrom <= time.AddMinutes(1)
                && time <= schedule.TimeTo)
            {
                return schedule.IdMooring;
            }
            return null;
        }

        public int? Visit(Reservation reservation)
        {
            TimeOnly time = TimeOnly.FromTimeSpan(Date.TimeOfDay);
            TimeOnly timeFrom = TimeOnly.FromTimeSpan(reservation.DateFrom.TimeOfDay);
            TimeOnly timeTo = timeFrom.AddHours(reservation.HoursDuration);

            if (reservation.DateFrom.Date == Date.Date && timeFrom <= time.AddMinutes(1) && time <= timeTo)
            {
                return reservation.IdMooring;
            }
            return null;
        }
    }
}
