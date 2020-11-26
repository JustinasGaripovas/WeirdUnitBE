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

        private static Tower CreateEmptyTowerWithType(string upgradableTowerType)
        {
            AbstractFactory defaultFactory = new DefaultTowerFactory();
            AbstractFactory fastFactory = new FastTowerFactory();
            AbstractFactory strongFactory = new StrongTowerFactory();

            if (String.Compare(upgradableTowerType, "DefaultRegeneratingTower") == 0)
            {
                return defaultFactory.CreateRegeneratingTower();
            }
            else if (String.Compare(upgradableTowerType, "DefaultAttackingTower") == 0)
            {
                return defaultFactory.CreateAttackingTower();
            }
            else if (String.Compare(upgradableTowerType, "FastRegeneratingTower") == 0)
            {
                return fastFactory.CreateRegeneratingTower();
            }
            else if (String.Compare(upgradableTowerType, "FastAttackingTower") == 0)
            {
                return fastFactory.CreateAttackingTower();
            }
            else if (String.Compare(upgradableTowerType, "StrongRegeneratingTower") == 0)
            {
                return strongFactory.CreateRegeneratingTower();
            }
            else if (String.Compare(upgradableTowerType, "StrongAttackingTower") == 0)
            {
                return strongFactory.CreateAttackingTower();
            }
            else
            {
                throw new InvalidUpgradeException("Cannot resolve Upgrade type");
            }
        }

        private static object FormatCommand(Tower tower)
        {
            JsonMessageFormatterTemplate formatter = new JsonUpgradeTowerMessageFormatter();
            var buffer = formatter.FormatJsonBufferFromParams(tower);

            return buffer;
        }
    }
}

