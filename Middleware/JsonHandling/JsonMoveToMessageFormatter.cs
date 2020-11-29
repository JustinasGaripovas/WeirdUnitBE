using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WeirdUnitBE.GameLogic.TowerPackage.Towers;
using WeirdUnitBE.Middleware;

namespace WeirdUnitBE.Middleware.JsonHandling
{
    public class JsonMoveToMessageFormatter : JsonMessageFormatterTemplate
    {
        protected override object FormatMessageObjectFromParams(params object[] parameters)
        {
            Tower towerFrom = (Tower)parameters[0];
            Tower towerTo = (Tower)parameters[1];
            int movementTimeInSeconds = (int)parameters[2];
            int movingUnitCount = (int)parameters[3];

            var messageObject = new
            {
                command = MoveToCommand(),
                payload = new
                {
                    towerToPosition = towerTo.position,
                    towerFromPosition = towerFrom.position,
                    unitCount = movingUnitCount,
                    uuidFrom = towerFrom.owner,
                    timeToArriveInSeconds = movementTimeInSeconds,
                    towerFromUnitCount = towerFrom.unitCount
                }
            };

            return messageObject;
        }

        private string MoveToCommand()
        {
            return Constants.JsonCommands.ServerCommands.MOVE_TO;
        }
    }
}