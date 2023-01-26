namespace ShippingPortGOF.Readers
{
    public interface IReader
    {
        public void ReadData(string path);

        public bool CheckHeaderRow(string path);
    }
}
