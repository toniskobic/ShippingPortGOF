using System.Text.RegularExpressions;
using ShippingPortGOF.Composite;
using ShippingPortGOF.Models;
using ShippingPortGOF.Singleton;
using ShippingPortGOF.Static;

namespace ShippingPortGOF.Readers
{
    public class MooringsReader : IReader
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

                    if (cells.Length != 7)
                    {
                        Print.ErrorCellsNumber(rows[i], path);
                    }
                    else
                    {
                        bool error = false;

                        for (int j = 0; j < cells.Length; j++)
                        {
                            if (j == 0 || j > 2)
                            {
                                if (!int.TryParse(cells[j], out _))
                                {
                                    error = true;
                                    Print.ErrorParseInt(rows[i], cells[j], path);
                                    break;
                                }
                            }
                            if (j == 2 && !Constants.MooringTypes.Contains(cells[j]))
                            {
                                error = true;
                                Print.ErrorInvalidType(rows[i], cells[j], path, "Mooring");
                                break;
                            }
                        }
                        if (!error)
                        {
                            int id = int.Parse(cells[0]);
                            int hourRate = int.Parse(cells[3]);
                            int maxLength = int.Parse(cells[4]);
                            int maxWidth = int.Parse(cells[5]);
                            int maxDepth = int.Parse(cells[6]);

                            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
                            List<IComponent> moorings = sps.ShippingPort!.Find(c => c is Mooring);
                            if (!moorings.Exists(x => x.GetId() == id)
                                && sps.ShippingPort.PortDepth >= maxDepth && Utils.CheckMoorings(cells[2]))
                            {
                                sps.ShippingPort.Add(new Mooring(id, cells[1], cells[2],
                                    hourRate, maxLength, maxWidth, maxDepth));
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
