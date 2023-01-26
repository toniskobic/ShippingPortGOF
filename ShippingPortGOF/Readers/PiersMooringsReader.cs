using System.Text.RegularExpressions;
using ShippingPortGOF.Composite;
using ShippingPortGOF.Models;
using ShippingPortGOF.Singleton;
using ShippingPortGOF.Static;

namespace ShippingPortGOF.Readers
{
    public class PiersMooringsReader : IReader
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
                        if (!int.TryParse(cells[0], out int idPier))
                        {
                            Print.ErrorParseInt(rows[i], cells[0], path);
                            continue;
                        }
                        if (!CheckMoorings(cells[1]))
                        {
                            Print.ErrorParseMoorings(rows[i], cells[1], path);
                            continue;
                        }
                        string[] records = cells[1].Split(",");
                        List<int> moorings = records.Select(int.Parse).ToList();

                        ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
                        List<IComponent> piers = sps.ShippingPort!.Find(c => c is Pier);
                        if (!piers.Exists(x => x.GetId() == idPier))
                        {
                            Print.ErrorInvalidRecord(rows[i], cells[0], path, idPier, "Pier");
                            continue;
                        }
                        foreach (int mooring in moorings)
                        {
                            List<IComponent> existingMoorings = sps.ShippingPort!.Find(c => c is Mooring);
                            Mooring? existingMooring = (Mooring?)existingMoorings.Find(x => x.GetId() == mooring);
                            if(existingMooring == null)
                            {
                                Print.ErrorInvalidRecord(rows[i], cells[1], path, mooring, "Mooring");
                                continue;
                            }
                            if(existingMooring.IdPier == null)
                            {
                                existingMooring.IdPier = idPier;
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

        public bool CheckMoorings(string moorings)
        {
            Regex rg = new Regex(Constants.Moorings);
            if (!rg.IsMatch(moorings))
            {
                return false;
            }

            return true;
        }
    }
}
