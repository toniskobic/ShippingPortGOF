namespace tskobic_zadaca_3.Modeli
{
    public class MolVez
    {
        public int IdMol { get; set; }

        public List<int> IdVezovi { get; set; }

        public MolVez(int idMol, List<int> idVezovi)
        {
            IdMol = idMol;
            IdVezovi = idVezovi;
        }
    }
}
