using System;
using System.Threading.Tasks;
using WeirdUnitBE.Middleware;

namespace WeirdUnitBE.Middleware.JsonHandling
{
    public class MoveToHandler : AbstractHandler
    {    
        public event EventHandler<JsonReceivedEventArgs> OnMoveToEvent;
        
        public override async Task Handle(Room room, dynamic jsonObj)
        {
            JsonReceivedEventArgs args = new JsonReceivedEventArgs(room, jsonObj);

            if (jsonObj.command == Constants.JsonCommands.ClientCommands.MOVE_TO)
            {
                await Task.Run(() => OnMoveToEvent?.Invoke(this, args));
            }       
            else
            {
                await base.Handle((Room)room, (object)jsonObj);
            }  
        }
    }
}