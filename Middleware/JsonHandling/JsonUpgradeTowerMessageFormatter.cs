using System;
using WeirdUnitBE.GameLogic.TowerPackage.Towers;
using WeirdUnitBE.Middleware;

namespace WeirdUnitBE.Middleware.JsonHandling
{
    public class JsonUpgradeTowerMessageFormatter : JsonMessageFormatterTemplate
    {
        protected override object FormatMessageObjectFromParams(params object[] parameters)
        {
            Tower tower = (Tower)parameters[0];

            var messageObject = new
                {
                    command = UpgradeTowerCommand(),
                    payload = new
                    {
                        owner = tower.owner,
                        position = tower.position,
                        unitCount = tower.unitCount,
                        neighbours = tower.neighbourTowers,
                        type = tower.type
                    }
                };

            return messageObject;
        }

        private string UpgradeTowerCommand()
        {
            return Constants.JsonCommands.ServerCommands.UPGRADE_TOWER;
        }
    }
}