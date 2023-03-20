namespace ShippingPortGOF.Models
{
    public class MooringTaken
    {
        public int ID { get; set; }

        public DateTime TakenFrom { get; set; }

        public DateTime TakenUntil { get; set; }

        public MooringTaken(int id, DateTime takenFrom, DateTime takenUntil)
        {
            ID = id;
            TakenFrom = takenFrom;
            TakenUntil = takenUntil;
        }
    }
}
