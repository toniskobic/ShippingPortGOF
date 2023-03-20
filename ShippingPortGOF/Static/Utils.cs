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
                        mooring = "PA";
                        break;
                    }
                case string shipType when Constants.BusinessShips.Contains(shipType):
                    {
                        mooring = "BU";
                        break;
                    }
                case string shipType when Constants.OtherShips.Contains(shipType):
                    {
                        mooring = "OT";
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
                case "PA":
                    {
                        ships = Constants.PassengerShips;
                        break;
                    }
                case "BU":
                    {
                        ships = Constants.BusinessShips;
                        break;
                    }
                case "OT":
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
                case "PA":
                    {
                        check = sps.ShippingPort.PassengerMoorings > moorings.FindAll(v => ((Mooring)v).Type == "PA").Count;
                        break;
                    }
                case "BU":
                    {
                        check = sps.ShippingPort.BusinessMoorings > moorings.FindAll(v => ((Mooring)v).Type == "BU").Count;
                        break;
                    }
                case "OT":
                    {
                        check = sps.ShippingPort.OtherMoorings > moorings.FindAll(v => ((Mooring)v).Type == "OT").Count;
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
