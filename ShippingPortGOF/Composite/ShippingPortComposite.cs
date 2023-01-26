using ShippingPortGOF.Iterator;
using ShippingPortGOF.Models;
using ShippingPortGOF.Observer;
using ShippingPortGOF.Visitor;

namespace ShippingPortGOF.Composite
{
    public class ShippingPortComposite : IComponent
    {
        #region Properties
        public List<IComponent> Children { get; set; }

        public int ID { get; set; }

        public string Name { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public int PortDepth { get; set; }

        public int PassengerMoorings { get; set; }

        public int BusinessMoorings { get; set; }

        public int OtherMoorings { get; set; }

        public List<Channel> Channels { get; set; }

        public List<Ship> Ships { get; set; }

        public List<Schedule> Schedules { get; set; }

        public List<Reservation> Reservations { get; set; }

        public List<MooredShip> MooredShips { get; set; }

        public List<Record> RecordLog { get; set; }
        #endregion

        #region Constructor
        public ShippingPortComposite(int id, string name, double latitude, double longitude, int portDepth,
            int passengerMoorings, int businessMoorings, int otherMoorings)
        {
            Children = new List<IComponent>();
            ID = id;
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
            PortDepth = portDepth;
            PassengerMoorings = passengerMoorings;
            BusinessMoorings = businessMoorings;
            OtherMoorings = otherMoorings;
            Channels = new List<Channel>();
            Ships = new List<Ship>();
            Schedules = new List<Schedule>();
            Reservations = new List<Reservation>();
            MooredShips = new List<MooredShip>();
            RecordLog = new List<Record>();
        }
        #endregion

        #region Methods
        public void Add(IComponent component)
        {
            Children.Add(component);
        }

        public void Remove(IComponent component)
        {
            Children.Remove(component);
        }

        public void RemoveAll(Predicate<IComponent> match)
        {
            Children.RemoveAll(match);
        }

        public int GetId()
        {
            return ID;
        }

        public string GetName()
        {
            return Name;
        }

        public bool IsComposite()
        {
            return true;
        }

        public List<IComponent> Find(Func<IComponent, bool> predicate)
        {
            IIterator iterator = CreateIterator();
            List<IComponent> list = new List<IComponent>();
            while (iterator.HasNext())
            {
                IComponent? component = (IComponent?)iterator.Next();
                if(predicate.Invoke(component!))
                {
                    list.Add(component!);
                }
            }

            return list;
        }

        public IIterator CreateIterator()
        {
            return new ComponentIterator(Children.GetEnumerator());
        }
        #endregion
    }
}
