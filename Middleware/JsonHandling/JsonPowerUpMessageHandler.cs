using System;
using WeirdUnitBE.GameLogic.TowerPackage.Towers;
using System.Collections;
using System.Collections.Generic;
using WeirdUnitBE.Middleware;

namespace WeirdUnitBE.Middleware.JsonHandling
{
    public class JsonPowerUpMessageFormatter : JsonMessageFormatterTemplate
    {
        protected override object FormatMessageObjectFromParams(params object[] parameters)
        {
            List<Tower> targetTowers = (List<Tower>)parameters[0];

            var messageObject = new
                {
                    command = PowerUpCommand(),
                    payload = new
                    {
                        allTowers = targetTowers
                    }
                };

            return messageObject;
        }

        private string PowerUpCommand()
        {
            return Constants.JsonCommands.ServerCommands.POWER_UP;
        }
    }
}