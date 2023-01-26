using System.Text.RegularExpressions;
using ShippingPortGOF.Composite;
using ShippingPortGOF.Singleton;
using ShippingPortGOF.Static;

namespace ShippingPortGOF.Readers
{
    public class PiersReader : IReader
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

                    if (cells.Length != 2)
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

                        ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
                        List<IComponent> piers = sps.ShippingPort!.Find(c => c is Pier);
                        if (!piers.Exists(x => x.GetId()== id))
                        {
                            sps.ShippingPort.Add(new Pier(id, cells[1]));
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
