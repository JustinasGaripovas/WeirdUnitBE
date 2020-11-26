using System;
using WeirdUnitBE.GameLogic.TowerPackage.Towers;
using WeirdUnitBE.Middleware;

namespace WeirdUnitBE.Middleware.JsonHandling
{
    public class JsonArrivedToMessageFormatter : JsonMessageFormatterTemplate
    {
        protected override object FormatMessageObjectFromParams(params object[] parameters)
        {
            Tower towerTo = (Tower)parameters[0];

            var messageObject = new
                {
                    command = ArrivedToCommand(),
                    payload = new
                    {
                        position = towerTo.position,
                        unitCount = towerTo.unitCount,
                        owner = towerTo.owner
                    }
                };

            return messageObject;
        }

        private string ArrivedToCommand()
        {
            return Constants.JsonCommands.ServerCommands.ARRIVED_TO;
        }
    }
}