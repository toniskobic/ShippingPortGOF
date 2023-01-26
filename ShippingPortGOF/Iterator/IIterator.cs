namespace ShippingPortGOF.Iterator
{
    public interface IIterator
    {
        public bool HasNext();

        public object? Next();   
    }
}
