using System;
using System.Collections.Generic;
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

            if (jsonObj.command == Constants.JsonCommands.ClientCommands.POWER_UP)
            {
                await Task.Run(() => OnPowerUpEvent?.Invoke(this, args));
                return;
            }

            if (jsonObj.command == Constants.JsonCommands.ClientCommands.MOVE_TO)
            {
                Console.WriteLine("We send move to back to the client");
                
                await Task.Run(() => OnMoveToEvent?.Invoke(this, args));
            }

            if (jsonObj.command == Constants.JsonCommands.ClientCommands.ARRIVED_TO)
            {
                await AnalyzeCommandBag(jsonObj, args);
            }

            _subject.commandList.Add(jsonObj as Object);
        }

        private async Task AnalyzeCommandBag(dynamic currentCommand, JsonReceivedEventArgs args)
        {
            foreach (dynamic command in _subject.commandList)
            {
                Console.WriteLine(IsSameCommand(currentCommand, command));

                if (IsSameCommand(currentCommand, command))
                {
                    if(currentCommand.command == Constants.JsonCommands.ClientCommands.ARRIVED_TO)
                    {
                        await Task.Run(() => OnMoveToEvent?.Invoke(this, args));
                    }
                }
            }
        }

        private bool IsSameCommand(dynamic currentCommand, dynamic command)
        {
            Console.WriteLine("STARTPROCESS;");
            Console.WriteLine(command.command);
            Console.WriteLine(currentCommand.command);
            Console.WriteLine(command.command == currentCommand.command);
            
            if (command.command == currentCommand.command)
            {
                Console.WriteLine("Commands are the same");
                
                JToken diffResult = _jdp.Diff(currentCommand, command);

                if(IsDiffNotEmpty(diffResult))
                {
                    // Console.WriteLine("IsDiffNotEmpty");
                    
                    List<string> jsonKeys = GetJsonKeyListFromJToken(diffResult);
                    // Console.WriteLine(diffResult.ToString());
                    //
                    // Console.WriteLine("------------------------------------------------");
                    //
                    // foreach (var VARIABLE in jsonKeys)
                    // {
                    //     Console.WriteLine(VARIABLE);
                    // }
                    
                    if (jsonKeys.Contains("uuid") && jsonKeys.Count == 1)
                    {
                        
                        // Console.WriteLine(jsonKeys.First().ToString());
                        
                        return true;
                    }
                }

                return false;
            }

            return false;
        }

        private List<string> GetJsonKeyListFromJToken(JToken diffResult)
        {
            List<string> jsonKeys = new List<string>();

            var jObject = JObject.Parse(diffResult["payload"].ToString());

            foreach (JProperty prop in jObject.Properties())
            {
                jsonKeys.Add(prop.Name);
            }

            return jsonKeys;
        }

        private bool IsDiffNotEmpty(JToken diffResult)
        {
            return diffResult != null;
        }

        public static object ConvertObjectToJsonBuffer(object obj)
        {
            var messageJson = JsonConvert.SerializeObject(obj, Formatting.Indented);
            var buffer = Encoding.UTF8.GetBytes(messageJson);
            return buffer;
        }
    }
}