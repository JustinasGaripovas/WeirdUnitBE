using System;
using System.Threading.Tasks;
using WeirdUnitBE.Middleware;

namespace WeirdUnitBE.Middleware.JsonHandling
{
    public class UpgradeTowerHandler : AbstractHandler
    {    
        public event EventHandler<JsonReceivedEventArgs> UpgradeTowerEvent;
        
        public override async Task Handle(Room room, dynamic jsonObj)
        {
            JsonReceivedEventArgs args = new JsonReceivedEventArgs(room, jsonObj);

            if (jsonObj.command == Constants.JsonCommands.ClientCommands.UPGRADE_TOWER)
            {
                await Task.Run(() => UpgradeTowerEvent?.Invoke(this, args));
            }       
            else
            {
                await base.Handle((Room)room, (object)jsonObj);
            }  
        }
    }
}