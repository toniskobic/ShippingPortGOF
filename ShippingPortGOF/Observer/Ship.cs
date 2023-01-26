using ShippingPortGOF.Singleton;

namespace ShippingPortGOF.Observer
{
    public class Ship : IObserver
    {
        #region Properties
        public int ID { get; set; }

        public string Label { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public double Length { get; set; }

        public double Width { get; set; }

        public double Draft { get; set; }

        public double MaxSpeed { get; set; }

        public int PassengersCapacity { get; set; }

        public int VehiclesCapacity { get; set; }

        public int CargoCapacity { get; set; }

        public string? LastMessage { get; set; }
        #endregion

        #region Constructor
        public Ship(int id, string label, string name, string type,
            double length, double width, double draft, double maxSpeed,
            int passengerCapacity, int vehiclesCapacity, int cargoCapacity)
        {
            ID = id;
            Label = label;
            Name = name;
            Type = type;
            Length = length;
            Width = width;
            Draft = draft;
            MaxSpeed = maxSpeed;
            PassengersCapacity = passengerCapacity;
            VehiclesCapacity = vehiclesCapacity;
            CargoCapacity = cargoCapacity;
        }
        #endregion

        #region Methods
        public void Update(ISubject s)
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            LastMessage = s.GetState();
            sps.Controller.SetModelState($"Ship {ID} received a message: '{LastMessage}'.");
        }
        #endregion
    }
}
