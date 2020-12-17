using System;
using System.Threading.Tasks;
using WeirdUnitBE.Middleware;

namespace WeirdUnitBE.Middleware.JsonHandling
{
    public class PowerUpHandler : AbstractHandler
    {    
        public event EventHandler<JsonReceivedEventArgs> OnPowerUpEvent;
        
        public override async Task Handle(Room room, dynamic jsonObj)
        {
            JsonReceivedEventArgs args = new JsonReceivedEventArgs(room, jsonObj);

            if (jsonObj.command == Constants.JsonCommands.ClientCommands.POWER_UP)
            {
                await Task.Run(() => OnPowerUpEvent?.Invoke(this, args));
            }       
            else
            {
                await base.Handle((Room)room, (object)jsonObj);
            }  
        }
    }
}