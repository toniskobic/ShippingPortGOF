using tskobic_zadaca_3.Iterator;

namespace tskobic_zadaca_3.Composite
{
    public interface IComponent
    {
        public int GetId();

        public string GetName();

        public bool IsComposite();
    }
}
