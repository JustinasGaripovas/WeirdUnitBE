using System;
using System.Collections;
using System.Collections.Generic;
using WeirdUnitBE.GameLogic.TowerPackage.Towers;
using WeirdUnitBE.GameLogic.Strategies;

namespace WeirdUnitBE.GameLogic
{
    public class ArrivedExecutive : IGameStateExecutable
    {
        public object ExecuteCommand(dynamic info, GameState gameState)
        {
            Tower towerFrom = info.towerFrom;
            Tower towerTo = info.towerTo;

            IMoveToStrategy strategy = (towerFrom.owner == towerTo.owner) 
                ? (IMoveToStrategy)new ReinforceStrategy() 
                : (IMoveToStrategy)new AttackStrategy();

            List<Tower> affectedTowers = new List<Tower>();
            strategy.ExecuteStrategy(towerFrom, towerTo, (affectedTowerFrom, affectedTowerTo) =>
            {
                affectedTowers = new List<Tower>()
                {
                    affectedTowerFrom,
                    affectedTowerTo
                };
            });
            gameState.UpdateTowers(affectedTowers);

            return affectedTowers;
        }
    }
}