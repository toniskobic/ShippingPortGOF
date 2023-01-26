namespace ShippingPortGOF.Models
{
    public class Record
    {
        public RequestType Request { get; set; }

        public int IdShip { get; set; }

        public bool Declined { get; set; }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }

        public Record(RequestType request, int idShip, bool declined,
            DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            Request = request;
            IdShip = idShip;
            Declined = declined;
            DateFrom = dateFrom;
            DateTo = dateTo;
        }
    }
}
