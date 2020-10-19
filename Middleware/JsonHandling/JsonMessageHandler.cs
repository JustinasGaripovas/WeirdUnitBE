using System;
using System.Threading;
using System.Threading.Tasks;

namespace WeirdUnitBE.Middleware.JsonHandling
{
    public class JsonMessageHandler
    {
        public event EventHandler<JsonReceivedEventArgs> OnMoveToEvent;

        public async Task HandleJsonMessage(string roomId, dynamic jsonObj)
        {
            JsonReceivedEventArgs args = new JsonReceivedEventArgs(roomId, jsonObj);
            
            if(jsonObj.command == Constants.JsonCommands.ClientCommands.MOVE_TO)
            {
                await Task.Run(() => OnMoveToEvent?.Invoke(this, args));
            }
        }

    }
}