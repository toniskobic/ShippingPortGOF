namespace ShippingPortGOF.Composite
{
    public interface IComponent
    {
        public int GetId();

        public string GetName();

        public bool IsComposite();
    }
}
