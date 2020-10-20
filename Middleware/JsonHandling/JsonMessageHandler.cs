using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WeirdUnitBE.Middleware;

namespace WeirdUnitBE.Middleware.JsonHandling
{
    public class JsonMessageHandler
    {
        public event EventHandler<JsonReceivedEventArgs> OnMoveToEvent;
        public event EventHandler<JsonReceivedEventArgs> OnPowerUpEvent;

        public async Task HandleJsonMessage(Room room, dynamic jsonObj)
        {
            JsonReceivedEventArgs args = new JsonReceivedEventArgs(room, jsonObj);
            
            if(jsonObj.command == Constants.JsonCommands.ClientCommands.MOVE_TO)
            {
                await Task.Run(() => OnMoveToEvent?.Invoke(this, args));
            }
            else if(jsonObj.command == Constants.JsonCommands.ClientCommands.POWER_UP)
            {
                await Task.Run(() => OnPowerUpEvent?.Invoke(this, args));
            }
        }

        public static object ConvertObjectToJsonBuffer(object obj)
        {
            var messageJson = JsonConvert.SerializeObject(obj, Formatting.Indented);
            var buffer = Encoding.UTF8.GetBytes(messageJson);
            return buffer;
        }
    }
}