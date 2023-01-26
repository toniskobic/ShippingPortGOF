using ShippingPortGOF.Singleton;
using ShippingPortGOF.Visitor;

namespace ShippingPortGOF.ChainOfResponsibility
{
    public class MooredShipHandler : BaseHandler
    {
        public MooredShipHandler() : base()
        {

        }

        public override void Handle(int idShip)
        {
            ShippingPortSingleton sps = ShippingPortSingleton.GetInstance();
            DateTime date = sps.VirtualTimeOriginator.VirtualTime;

            MooredShip? mooredShip = sps.ShippingPort!.MooredShips.Find(x => x.IdShip == idShip
                && x.DateFrom <= date && date <= x.DateTo);
            if (mooredShip != null)
            {
                sps.Controller.SetModelState($"Ship {idShip} status: Currently is moored on mooring {mooredShip.IdMooring}.");
            }
            else
            {
                sps.Controller.SetModelState($"Ship {idShip} status: Currently is not moored.");
                if(NextHandler != null)
                {
                    NextHandler.Handle(idShip);
                }
            }
        }
    }
}
