using ShippingPortGOF.Readers;

namespace ShippingPortGOF.FactoryMethod
{
    public abstract class Creator
    {
        private IReader? reader;

        public abstract IReader FactoryMethod();

        public void ReadData(string path)
        {
            reader = FactoryMethod();
            reader.ReadData(path);
        }
    }
}
