using tskobic_zadaca_3.Modeli;

namespace tskobic_zadaca_3.Visitor
{
    public class VezVisitor : IVezVisitor
    {
        public string Visit(Vez vez)
        {
            return vez.Vrsta;
        }
    }
}
