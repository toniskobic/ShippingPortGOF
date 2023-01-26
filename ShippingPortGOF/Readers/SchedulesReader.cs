using System.Text.RegularExpressions;
using ShippingPortGOF.Composite;
using ShippingPortGOF.Models;
using ShippingPortGOF.Observer;
using ShippingPortGOF.Singleton;
using ShippingPortGOF.Static;
using ShippingPortGOF.Visitor;

namespace ShippingPortGOF.Readers
{
    public class SchedulesReader : IReader
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

                for (int i = 1; i < rows.Length; i++)
                {
                    string[] cells = rows[i].Split(";");
                    if (cells.Length != 5)
                    {
                        Print.ErrorCellsNumber(rows[i], path);
                    }
                    else
                    {
                        if (!int.TryParse(cells[0], out int idMooring))
                        {
                            Print.ErrorParseInt(rows[i], cells[0], path);
                            continue;
                        }
                        if (!int.TryParse(cells[1], out int idShip))
                        {
                            Print.ErrorParseInt(rows[i], cells[1], path);
                            continue;
                        }
                        List<string> days = cells[2].Split(",").ToList();
                        if (days.Count > 7)
                        {
                            Print.ErrorParseDaysOfWeek(rows[i], cells[2], path);
                            continue;
                        }
                        bool error = false;
                        List<DayOfWeek> daysOfWeek = new List<DayOfWeek>();
                        foreach (var day in days)
                        {
                            if (!int.TryParse(day, out int result))
                            {
                                error = true;
                                Print.ErrorParseDaysOfWeek(rows[i], cells[2], path);
                                break;
                            }
                            daysOfWeek.Add((DayOfWeek)result);
                        }
                        if (error)
                        {
                            continue;
                        }
                        if (days.FindAll(x => int.Parse(x) < 0 || int.Parse(x) > 6).Count != 0)
                        {
                            Print.ErrorParseDaysOfWeek(rows[i], cells[2], path);
                            continue;
                        }
                        if (!TimeOnly.TryParse(cells[3], out TimeOnly timeFrom))
                        {
                            Print.ErrorParseHours(rows[i], cells[3], path);
                            continue;
                        }
                        if (!TimeOnly.TryParse(cells[4], out TimeOnly timeTo))
                        {
                            Print.ErrorParseHours(rows[i], cells[4], path);
                            continue;
                        }
                        if (timeFrom >= timeTo)
                        {
                            continue;
                        }
                        ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
                        List<IComponent> moorings = sps.ShippingPort!.Find(c => c is Mooring).ToList();
                        Ship? ship = sps.ShippingPort.Ships.Find(s => s.ID == idShip);
                        Mooring? mooring = (Mooring?)moorings.Find(m => m.GetId() == idMooring);
                        if (ship != null && mooring != null && Utils.CheckShipAndMooring(ship, mooring)
                            && !sps.ShippingPort.Schedules.Exists(s => s.IdShip == ship.ID && s.IdMooring == mooring.ID
                            && daysOfWeek.Any(d => s.DaysOfWeek.Any(y => y == d)
                            && timeFrom <= s.TimeTo && s.TimeFrom <= timeTo)))
                        {
                            sps.ShippingPort.Schedules.Add(new Schedule(idMooring, idShip, daysOfWeek, timeFrom, timeTo));
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
