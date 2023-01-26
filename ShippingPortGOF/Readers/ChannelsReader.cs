using System.Text.RegularExpressions;
using ShippingPortGOF.Observer;
using ShippingPortGOF.Singleton;
using ShippingPortGOF.Static;

namespace ShippingPortGOF.Readers
{
    internal class ChannelsReader : IReader
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

                    if (cells.Length != 3)
                    {
                        Print.ErrorCellsNumber(rows[i], path);
                    }
                    else
                    {
                        if (!int.TryParse(cells[0], out int id))
                        {
                            Print.ErrorParseInt(rows[i], cells[0], path);
                            continue;
                        }
                        if (!int.TryParse(cells[1], out int frequency))
                        {
                            Print.ErrorParseInt(rows[i], cells[1], path);
                            continue;
                        }
                        if (!int.TryParse(cells[2], out int maxConnections))
                        {
                            Print.ErrorParseInt(rows[i], cells[2], path);
                            continue;
                        }

                        ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
                        if (!sps.ShippingPort!.Channels.Exists(x => x.ID == id || x.Frequency == frequency))
                        {
                            sps.ShippingPort.Channels.Add(new Channel(id, frequency, maxConnections));
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
