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
using System.Runtime.CompilerServices;

namespace WeirdUnitBE.Middleware.JsonHandling
{
    public class ArrivedToHandler : AbstractHandler
    {    
        public event EventHandler<JsonReceivedEventArgs> OnArrivedToEvent;
        private readonly RoomSubject _subject;
        private readonly JsonDiffPatch _jdp;

        public ArrivedToHandler(RoomSubject subject)
        {
            _jdp = new JsonDiffPatch();
            _subject = subject;
        }

        public override async Task Handle(Room room, dynamic jsonObj)
        {
            JsonReceivedEventArgs args = new JsonReceivedEventArgs(room, jsonObj);

            if (jsonObj.command == Constants.JsonCommands.ClientCommands.ARRIVED_TO)
            {
                await AnalyzeCommandBagAsync(jsonObj, args);
            }       
            else
            {
                await base.Handle((Room)room, (object)jsonObj);
            }  
        }

        private async Task AnalyzeCommandBagAsync(dynamic currentCommand, JsonReceivedEventArgs args)
        {                      
            if(SimilarCommandsByDifferentSenders(currentCommand))
            {
                await Task.Run(() => OnArrivedToEvent?.Invoke(this, args));
            }           
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool SimilarCommandsByDifferentSenders(dynamic currentCommand)
        {
            foreach (dynamic command in _subject.commandList)
            {                     
                if (SimilarCommandFound(currentCommand, command))
                {   
                    return true;
                }
            }           
            _subject.commandList.Add(currentCommand as Object);
            return false;
        }

        private bool SimilarCommandFound(dynamic command1, dynamic command2)
        {
            JToken JTokenDifference = _jdp.Diff(command1, command2);
            try
            {
                List<string> jsonKeysFromDifference = GetJsonKeyListFromJToken(JTokenDifference);
                if(ListContainsOnlyOneSpecificDifference(jsonKeysFromDifference, "uuid")) {
                    return true;
                }
            }
            catch(Exception) {
            }
            return false;        
        }

        private bool ListContainsOnlyOneSpecificDifference(List<string> list, string difference)
        {
            return list.Count == 1 && list.Contains(difference);
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
    }
}