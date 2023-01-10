using tskobic_zadaca_3.Modeli;
using tskobic_zadaca_3.Singleton;

namespace tskobic_zadaca_3.ChainOfResponsibility
{
    public class RasporedHandler : BaseHandler
    {
        public RasporedHandler() : base()
        {

        }

        public override void Handle(int idBrod)
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            DateTime datum = bls.VirtualniSat.VirtualnoVrijeme;
            TimeOnly vrijeme = TimeOnly.FromTimeSpan(datum.TimeOfDay);
            DayOfWeek dan = datum.DayOfWeek;

            Raspored? raspored = bls.BrodskaLuka!.Rasporedi.Find(x => x.IdBrod == idBrod
                && x.DaniUTjednu.Contains(dan) && x.VrijemeOd <= vrijeme && x.VrijemeDo > vrijeme);
            if (raspored != null)
            {
                Console.WriteLine($"Status broda {idBrod}: Trenutno ima rezervaciju prema rasporedu");
            } else
            {
                Console.WriteLine($"Status broda {idBrod}: Trenutno nema rezervaciju prema rasporedu.");
                if (NextHandler != null)
                {
                    NextHandler.Handle(idBrod);
                }
            }
        }
    }
}
