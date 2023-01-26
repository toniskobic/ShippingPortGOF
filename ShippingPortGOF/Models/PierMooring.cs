namespace ShippingPortGOF.Models
{
    public class PierMooring
    {
        public int IdPier { get; set; }

        public List<int> IdMoorings { get; set; }

        public PierMooring(int idPier, List<int> idMoorings)
        {
            IdPier = idPier;
            IdMoorings = idMoorings;
        }
    }
}
