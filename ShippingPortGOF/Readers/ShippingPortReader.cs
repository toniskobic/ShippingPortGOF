using System.Globalization;
using System.Text.RegularExpressions;
using ShippingPortGOF.Composite;
using ShippingPortGOF.Singleton;
using ShippingPortGOF.Static;

namespace ShippingPortGOF.Readers
{
    public class ShippingPortReader : IReader
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
                string[] cells = rows[1].Split(";");
 
                if (cells.Length != 8)
                {
                    Print.ErrorCellsNumber(rows[1], path);
                }
                for (int i = 1; i < cells.Length; i++)
                {
                    if (i == 1 || i == 2)
                    {
                        if (!Utils.CheckDoubleParse(cells[i]))
                        {
                            Print.ErrorParseDouble(rows[1], cells[i], path);
                            return;
                        }
                    }
                    if (i > 2 && i < 7 && !int.TryParse(cells[i], out _))
                    {
                        Print.ErrorParseInt(rows[1], cells[i], path);
                        return;
                    }
                }
                if (!Utils.CheckDateParse(cells[7], out DateTime virtualTime))
                {
                    Print.ErrorParseDate(rows[1], cells[7], path);
                    return;
                }

                double latitude = double.Parse(cells[1], new CultureInfo("hr-hr"));
                double longitude = double.Parse(cells[2], new CultureInfo("hr-hr"));
                int portDepth = int.Parse(cells[3]);
                int passengerMoorings = int.Parse(cells[4]);
                int businessMoorings = int.Parse(cells[5]);
                int otherMoorings = int.Parse(cells[6]);

                ShippingPortComposite shippingPort = new ShippingPortComposite(1, cells[0], latitude, longitude, portDepth, passengerMoorings, businessMoorings, otherMoorings);
                ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
                sps.ShippingPort = shippingPort;
                sps.VirtualTimeOriginator.VirtualTime = virtualTime;
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
