using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeirdUnitBE.GameLogic.TowerPackage.Towers;
using WeirdUnitBE.GameLogic.TowerPackage.Factories;
using WeirdUnitBE.GameLogic.TowerPackage.Factories.ConcreteFactories;

namespace WeirdUnitBE.GameLogic
{
    public class UpgradeTowerExecutive : IGameStateExecutable
    {
        public object ExecuteCommand(dynamic args, GameState gameState)
        {
            // GetPayloadFromArgs
            // GetTowersFromJsonArgs
            // UpgradeTower
            // FormatCommand
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
            AbstractFactory towerFactory = new DefaultTowerFactory();
            
            if(String.Compare(upgradableTowerType, "Regenerating") == 0) {
                return towerFactory.CreateRegeneratingTower();  
            }
            else if(String.Compare(upgradableTowerType, "Attacking") == 0) {
                return towerFactory.CreateAttackingTower();
            }
            else {
                throw new InvalidUpgradeException("Cannot resolve Upgrade type");
            }           
        }

        private static object FormatCommand(Tower tower)
        {
            return new
            {
                command = Constants.JsonCommands.ServerCommands.UPGRADE_TOWER,
                payload = new
                {
                    owner = tower.owner,
                    position = tower.position,
                    unitCount = tower.unitCount,
                    neighbours = tower.neighbourTowers,
                    type = tower.type
                }
            };
        }
    }
}

