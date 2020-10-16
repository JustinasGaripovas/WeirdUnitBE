using System;
using System.Threading;
using System.Threading.Tasks;

namespace WeirdUnitBE.Middleware
{
    public class JsonMessageHandler
    {
        public event EventHandler<JsonReceivedEventArgs> OnMoveToEvent;

        public async Task HandleJsonMessage(string roomId, dynamic jsonObj)
        {
            JsonReceivedEventArgs args = new JsonReceivedEventArgs(roomId, jsonObj);
            if(jsonObj.command == "c:MoveTo")
            {
                await Task.Run(() => OnMoveToEvent?.Invoke(this, args));
            }
        }

    }
}