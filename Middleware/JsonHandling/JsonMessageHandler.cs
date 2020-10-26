using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JsonDiffPatchDotNet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WeirdUnitBE.Middleware;
using WeirdUnitBE.Middleware.Observable.ConcreteSubjects;

namespace WeirdUnitBE.Middleware.JsonHandling
{
    public class JsonMessageHandler
    {
        private readonly RoomSubject _subject;
        private readonly JsonDiffPatch _jdp;
        public event EventHandler<JsonReceivedEventArgs> OnMoveToEvent;
        public event EventHandler<JsonReceivedEventArgs> OnPowerUpEvent;

        public JsonMessageHandler(RoomSubject subject)
        {
            _jdp = new JsonDiffPatch();
            _subject = subject;
        }
        
        public async Task HandleJsonMessage(Room room, dynamic jsonObj)
        {
            JsonReceivedEventArgs args = new JsonReceivedEventArgs(room, jsonObj);

            if(jsonObj.command == Constants.JsonCommands.ClientCommands.POWER_UP)
            {
                await Task.Run(() => OnPowerUpEvent?.Invoke(this, args));
                return;
            }

            if(jsonObj.command == Constants.JsonCommands.ClientCommands.MOVE_TO)
            {
                await Task.Run(() => OnMoveToEvent?.Invoke(this, args));
            }
            
            await AnalyzeCommandBag(jsonObj, args);
            
            _subject.commandList.Add(jsonObj as Object);

        }

        private async Task AnalyzeCommandBag(dynamic currentCommand, JsonReceivedEventArgs args)
        {
            foreach (dynamic command in _subject.commandList)
            {
                if (IsSameCommand(currentCommand, command))
                {
                    if(currentCommand.command == Constants.JsonCommands.ClientCommands.ARRIVED)
                    {
                        await Task.Run(() => OnMoveToEvent?.Invoke(this, args));
                    }
                }
            }
        }

        private bool IsSameCommand(dynamic currentCommand, dynamic command)
        {
            Console.WriteLine(command??"Empty command");
            Console.WriteLine(currentCommand??"Empty currentCommand");
            
            if (command.command == currentCommand.command)
            {
                
                
                JToken diffResult = _jdp.Diff(currentCommand, command);
                Console.WriteLine(diffResult.ToString());
                return true;
            }

            return false;
        }
        
        public static object ConvertObjectToJsonBuffer(object obj)
        {
            var messageJson = JsonConvert.SerializeObject(obj, Formatting.Indented);
            var buffer = Encoding.UTF8.GetBytes(messageJson);
            return buffer;
        }
    }
}