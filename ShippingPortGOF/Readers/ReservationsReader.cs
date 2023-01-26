using System.Text.RegularExpressions;
using ShippingPortGOF.Composite;
using ShippingPortGOF.Models;
using ShippingPortGOF.Observer;
using ShippingPortGOF.Singleton;
using ShippingPortGOF.Static;
using ShippingPortGOF.Visitor;

namespace ShippingPortGOF.Readers
{
    public class ReservationsReader : IReader
    {
        public void ReadData(string path)
        {
            try
            {
                if (!CheckHeaderRow(path))
                {
                    return;
                }
                string[] rows = File.ReadAllLines(path);

                for (int i = 0; i < rows.Length; i++)
                {
                    string[] cells = rows[i].Split(";");
                    if (cells.Length != 3)
                    {
                        Print.ErrorCellsNumber(rows[i], path);
                    }
                    else
                    {
                        if (!int.TryParse(cells[0], out int idShip))
                        {
                            Print.ErrorParseInt(rows[i], cells[0], path);
                            continue;
                        }
                        if (!Utils.CheckDateParse(cells[1], out DateTime dateFrom))
                        {
                            Print.ErrorParseDate(rows[i], cells[1], path);
                            continue;
                        }
                        if (!int.TryParse(cells[2], out int mooringDuration))
                        {
                            Print.ErrorParseInt(rows[i], cells[2], path);
                            continue;
                        }
                        TimeOnly timeFrom = TimeOnly.FromTimeSpan(dateFrom.TimeOfDay);
                        if (timeFrom >= timeFrom.AddHours(mooringDuration))
                        {
                            continue;
                        }
                        TimeOnly timeTo = timeFrom.AddHours(mooringDuration);
                        ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
                        Ship? ship = sps.ShippingPort!.Ships.Find(x => x.ID == idShip);
                        if (ship != null)
                        {
                            List<Mooring> moorings = Utils.FindMoorings(ship);
                            if (moorings.Count > 0)
                            {
                                List<Mooring> fMooringsSchedules = moorings.FindAll(m => sps.ShippingPort.Schedules.Any(s => s.IdMooring == m.ID
                                && s.DaysOfWeek.Contains(dateFrom.DayOfWeek) && s.TimeFrom <= timeTo && timeFrom <= s.TimeTo));

                                List<Mooring> fMooringsReservations = moorings.FindAll(m => sps.ShippingPort.Reservations.Any(r => r.IdMooring == m.ID
                                && r.DateFrom.Date == dateFrom.Date
                                && TimeOnly.FromTimeSpan(r.DateFrom.TimeOfDay) <= timeTo
                                && timeFrom <= TimeOnly.FromTimeSpan(r.DateFrom.TimeOfDay).AddHours(r.HoursDuration)));

                                List<Mooring> availableMoorings = moorings.Except(fMooringsSchedules).Except(fMooringsReservations).ToList();

                                if (availableMoorings.Count > 0)
                                {
                                    Mooring mooring = Utils.FindOptimalMooring(moorings);
                                    sps.ShippingPort.Reservations.Add(new Reservation(mooring.ID, idShip, dateFrom, mooringDuration));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        public bool CheckHeaderRow(string path)
        {
            Regex rg = new Regex(Constants.HeaderRow);
            string row = File.ReadLines(path).First();
            if (!rg.IsMatch(row))
            {
                Print.ErrorInvalidHeaderRow(row, path);
                return false;
            }

            return true;
        }
    }
}
