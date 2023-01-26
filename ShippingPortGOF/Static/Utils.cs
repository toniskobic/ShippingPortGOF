using System.Globalization;
using ShippingPortGOF.Composite;
using ShippingPortGOF.Models;
using ShippingPortGOF.Observer;
using ShippingPortGOF.Singleton;

namespace ShippingPortGOF.Static
{
    public static class Utils
    {
        public static bool CheckDoubleParse(string cell)
        {
            return double.TryParse(cell, NumberStyles.Float, new CultureInfo("hr-hr"), out _);
        }

        public static bool CheckDateParse(string cell, out DateTime date)
        {
            return DateTime.TryParse(cell, new CultureInfo("hr-hr"), DateTimeStyles.None, out date);
        }

        public static List<Mooring> FindMoorings(Ship ship)
        {
            string? mooring = null;
            switch (ship.Type)
            {
                case string shipType when Constants.PassengerShips.Contains(shipType):
                    {
                        mooring = "PU";
                        break;
                    }
                case string shipType when Constants.BusinessShips.Contains(shipType):
                    {
                        mooring = "PO";
                        break;
                    }
                case string shipType when Constants.OtherShips.Contains(shipType):
                    {
                        mooring = "OS";
                        break;
                    }
            }
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            List<Mooring> moorings = sps.ShippingPort!.Find(c => c is Mooring).Cast<Mooring>().ToList();
            List<Mooring> suitableMoorings = moorings.FindAll(v => v.Type == mooring);
            return suitableMoorings.FindAll(v => CheckShipAndMooring(ship, v));
        }

        public static bool CheckShipAndMooring(Ship ship, Mooring mooring)
        {
            bool check = false;
            string[]? ships = null;
            switch (mooring.Type)
            {
                case "PU":
                    {
                        ships = Constants.PassengerShips;
                        break;
                    }
                case "PO":
                    {
                        ships = Constants.BusinessShips;
                        break;
                    }
                case "OS":
                    {
                        ships = Constants.OtherShips;
                        break;
                    }
            }
            if (ships!.Contains(ship.Type)
                    && mooring.MaxDepth > ship.Draft && mooring.MaxWidth >= ship.Width && mooring.MaxLength >= ship.Length)
            {
                check = true;
            }

            return check;
        }

        public static bool CheckMoorings(string type)
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            List<IComponent> moorings = sps.ShippingPort!.Find(c => c is Mooring);

            bool check = false;

            switch (type)
            {
                case "PU":
                    {
                        check = sps.ShippingPort.PassengerMoorings > moorings.FindAll(v => ((Mooring)v).Type == "PU").Count;
                        break;
                    }
                case "PO":
                    {
                        check = sps.ShippingPort.BusinessMoorings > moorings.FindAll(v => ((Mooring)v).Type == "PO").Count;
                        break;
                    }
                case "OS":
                    {
                        check = sps.ShippingPort.OtherMoorings > moorings.FindAll(v => ((Mooring)v).Type == "OS").Count;
                        break;
                    }
            }

            return check;
        }

        public static Mooring FindOptimalMooring(List<Mooring> moorings)
        {
            return moorings.Aggregate((min, next) =>
            {
                if (min.Volume == next.Volume)
                {
                    return min.HourRate <= next.HourRate ? min : next;
                }
                return min.Volume < next.Volume ? min : next;
            });
        }
    }
}
