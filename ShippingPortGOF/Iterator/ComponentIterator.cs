using System.Collections;

namespace ShippingPortGOF.Iterator
{
    public class ComponentIterator : IIterator
    {
        private bool fetchedNext = false;
        public bool nextAvailable = false;
        private object? next;

        public IEnumerator Iterator { get; set; }

        public ComponentIterator(IEnumerator enumerator)
        {
            Iterator = enumerator;
        }

        public bool HasNext()
        {
            CheckNext();
            return nextAvailable;
        }

        public object? Next()
        {
            CheckNext();
            if (!nextAvailable)
            {
                return null;
            }
            fetchedNext = false;
            return next;
        }

        void CheckNext()
        {
            if (!fetchedNext)
            {
                nextAvailable = Iterator.MoveNext();
                if (nextAvailable)
                {
                    next = Iterator.Current;
                }
                fetchedNext = true;
            }
        }
    }
}
