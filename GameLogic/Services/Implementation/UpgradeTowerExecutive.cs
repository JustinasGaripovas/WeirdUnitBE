using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeirdUnitBE.GameLogic.TowerPackage.Towers;
using WeirdUnitBE.GameLogic.TowerPackage.Factories;
using WeirdUnitBE.GameLogic.TowerPackage.Factories.ConcreteFactories;
using WeirdUnitBE.Middleware.JsonHandling;

namespace WeirdUnitBE.GameLogic
{
    public class UpgradeTowerExecutive : IGameStateExecutable
    {
        public object ExecuteCommand(dynamic args, GameState gameState)
        {
            dynamic payload = args.jsonObj.payload;
            Position towerPosition = new Position((int)payload.position.X, (int)payload.position.Y);
            Tower upgradableTower = gameState.PositionToTowerDict[towerPosition];

            UpgradeTower(payload, ref upgradableTower);
            return FormatCommand(upgradableTower);
        }

        private static void UpgradeTower(dynamic payload, ref Tower tower)
        {
            string upgradeableTowerType = (string)payload.upgradeToType;
            Tower newTower = CreateEmptyTowerWithType(upgradeableTowerType);
            newTower.position = tower.position;
            newTower.unitCount = tower.unitCount;
            newTower.owner = tower.owner;
            newTower.neighbourTowers = tower.neighbourTowers;

            tower = newTower;
        }

        private static AbstractFactory CreateTowerFactory(string upgradableTowerType)
        {
            if (upgradableTowerType.Contains("Default"))
                return new DefaultTowerFactory();

            if (upgradableTowerType.Contains("Fast"))
                return new FastTowerFactory();
            
            return new StrongTowerFactory();
        }

        private static Tower CreateEmptyTowerWithType(string upgradableTowerType)
        {
            AbstractFactory towerFactory = CreateTowerFactory(upgradableTowerType);

            if (upgradableTowerType.Contains("Regenerating"))
                return towerFactory.CreateRegeneratingTower();
            
            return towerFactory.CreateAttackingTower();
        }

        private static object FormatCommand(Tower tower)
        {
            JsonMessageFormatterTemplate formatter = new JsonUpgradeTowerMessageFormatter();
            var buffer = formatter.FormatJsonBufferFromParams(tower);

            return buffer;
        }
    }
}

