using System;
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
            Position towerPosition = new Position((int)args.position.X, (int)args.position.Y);
            Tower upgradableTower = gameState.PositionToTowerDict[towerPosition];

            AbstractFactory abstractTowerFactory = new DefaultTowerFactory();

            Tower newTower;
            if (global::System.String.Compare((string)args.type, "Regenerating") == 0)
                newTower = abstractTowerFactory.CreateRegeneratingTower();
            else
                newTower = abstractTowerFactory.CreateAttackingTower();

            newTower.position = towerPosition;
            newTower.unitCount = upgradableTower.unitCount;
            newTower.owner = upgradableTower.owner;
            newTower.neighbourTowers = upgradableTower.neighbourTowers;

            List<Tower> affectedTowers;
            affectedTowers = new List<Tower>()
            {
                newTower
            };

            gameState.UpdateTowers(affectedTowers);

            return affectedTowers;
        }
    }
}
