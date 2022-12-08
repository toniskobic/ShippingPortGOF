using tskobic_zadaca_2.Modeli;
using tskobic_zadaca_2.Singleton;

namespace tskobic_zadaca_2.ChainOfResponsibility
{
    public class PrivezHandler : BaseHandler
    {
        public PrivezHandler() : base()
        {

        }

        public override void Handle(int idBrod)
        {
            BrodskaLukaSingleton bls = BrodskaLukaSingleton.Instanca();
            DateTime datum = bls.VirtualniSat.VirtualnoVrijeme;
            TimeOnly vrijeme = TimeOnly.FromTimeSpan(datum.TimeOfDay);
            DayOfWeek dan = datum.DayOfWeek;

            Privez? privez = bls.BrodskaLuka!.Privezi.Find(x => x.IdBrod == idBrod
                && x.VrijemeOd.Date.Equals(datum.Date)
                && TimeOnly.FromTimeSpan(x.VrijemeOd.TimeOfDay) <= vrijeme
                && TimeOnly.FromTimeSpan(x.VrijemeDo.TimeOfDay) > vrijeme);
            if (privez != null)
            {
                Console.WriteLine($"Status broda {idBrod}: Trenutno je privezan na vez {privez.IdVez}.");
            }
            else
            {
                Console.WriteLine($"Status broda {idBrod}: Treutno nije privezan.");
                if(NextHandler != null)
                {
                    NextHandler.Handle(idBrod);
                }
            }
        }
    }
}
