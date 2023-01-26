using System.Globalization;
using System.Text.RegularExpressions;
using ShippingPortGOF.Composite;
using ShippingPortGOF.Observer;
using ShippingPortGOF.Singleton;
using ShippingPortGOF.Static;

namespace ShippingPortGOF.Readers
{
    public class ShipsReader : IReader
    {
        public void ReadData(string path)
        {
            try
            {
                if (!CheckHeaderRow(path))
                {
                    return;
                }
                string[] row = File.ReadAllLines(path);

                for (int i = 1; i < row.Length; i++)
                {
                    string[] cells = row[i].Split(";");

                    if (cells.Length != 11)
                    {
                        Print.ErrorCellsNumber(row[i], path);
                    }
                    else
                    {
                        bool error = false;

                        for (int j = 0; j < cells.Length; j++)
                        {
                            if (j == 0 || j > 7)
                            {
                                if (!int.TryParse(cells[j], out _))
                                {
                                    error = true;
                                    Print.ErrorParseInt(row[i], cells[j], path);
                                    break;
                                }
                            }
                            if (j == 3 && !Constants.ShipTypes.Contains(cells[j]))
                            {
                                error = true;
                                Print.ErrorInvalidType(row[i], cells[j], path, "Ship");
                                break;
                            }
                            if (j > 3 && j < 8 && !Utils.CheckDoubleParse(cells[j]))
                            {
                                error = true;
                                Print.ErrorParseDouble(row[i], cells[j], path);
                                break;
                            }
                        }
                        if (!error)
                        {
                            int id = int.Parse(cells[0]);
                            double length = double.Parse(cells[4], new CultureInfo("hr-hr"));
                            double width = double.Parse(cells[5], new CultureInfo("hr-hr"));
                            double shipDraft = double.Parse(cells[6], new CultureInfo("hr-hr"));
                            double maxSpeed = double.Parse(cells[7], new CultureInfo("hr-hr"));
                            int passengerCapacity = int.Parse(cells[8]);
                            int vehiclesCapacity = int.Parse(cells[9]);
                            int cargoCapacity = int.Parse(cells[10]);

                            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
                            if (!sps.ShippingPort!.Ships.Exists(x => x.ID == id) && sps.ShippingPort.PortDepth > shipDraft)
                            {
                                Ship ship = new Ship(id, cells[1], cells[2], cells[3],
                                    length, width, shipDraft, maxSpeed, passengerCapacity,
                                    vehiclesCapacity, cargoCapacity);
                                sps.ShippingPort.Ships.Add(ship);
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
