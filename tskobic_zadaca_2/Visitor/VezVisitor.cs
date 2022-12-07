using tskobic_zadaca_2.Modeli;

namespace tskobic_zadaca_2.Visitor
{
    public class VezVisitor : IVezVisitor
    {
        public string Visit(Vez vez)
        {
            return vez.Vrsta;
        }
    }
}
