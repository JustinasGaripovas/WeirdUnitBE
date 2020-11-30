using System;
using WeirdUnitBE.Middleware;
using WeirdUnitBE.GameLogic;

namespace WeirdUnitBE.Middleware.JsonHandling
{
    public class JsonInitialGameStateMessageFormatter : JsonMessageFormatterTemplate
    {
        protected override object FormatMessageObjectFromParams(params object[] parameters)
        {
            string roomId = (string)parameters[0];
            GameState gameState = (GameState)parameters[1];

            var messageObject = new
                {
                    command = InitialCommand(),
                    payload = new
                    {
                        roomId = roomId,
                        mapX = gameState.GetMapDimensions().X,
                        mapY = gameState.GetMapDimensions().Y,
                        allTowers = gameState.GetAllTowers(),
                        allPowerUps = gameState.GetAllPowerUps()
                    }
                };

            return messageObject;
        }

        private string InitialCommand()
        {
            return Constants.JsonCommands.ServerCommands.INITIAL;
        }
    }
}