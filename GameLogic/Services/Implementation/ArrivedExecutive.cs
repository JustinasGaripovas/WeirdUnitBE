using System;
using System.Collections;
using System.Collections.Generic;
using WeirdUnitBE.GameLogic.TowerPackage.Towers;
using WeirdUnitBE.GameLogic.Strategies;

namespace WeirdUnitBE.GameLogic
{
    public class ArrivedExecutive : IGameStateExecutable
    {
        public object ExecuteCommand(dynamic args, GameState gameState)
        {
            (Tower towerFrom, Tower towerTo) = GetTowerParameters(args as Object, gameState);

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

            return FormatCommand(affectedTowers);
        }

        private (Tower, Tower) GetTowerParameters(dynamic args, GameState gameState)
        {
            Position positionFrom = new Position((int) args.jsonObj.moveFrom.X, (int) args.jsonObj.moveFrom.Y);
            Tower towerFrom = gameState.PositionToTowerDict[positionFrom];

            Position positionTo = new Position((int) args.moveTo.X, (int) args.moveTo.Y);
            Tower towerTo = gameState.PositionToTowerDict[positionTo];

            return (towerFrom, towerTo);
        }

        private dynamic FormatCommand(dynamic affectedTowers)
        {
            return new
            {
                command = Constants.JsonCommands.ServerCommands.MOVE_TO,
                payload = new { allTowers = affectedTowers }
            };
        }
    }
}