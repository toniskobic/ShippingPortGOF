using tskobic_zadaca_3.Modeli;
using tskobic_zadaca_3.Singleton;

namespace tskobic_zadaca_3.ChainOfResponsibility
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

            Privez? privez = bls.BrodskaLuka!.Privezi.Find(x => x.IdBrod == idBrod
                && x.VrijemeOd <= datum && datum <= x.VrijemeDo);
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
