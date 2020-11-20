using System;
using System.Collections;
using System.Collections.Generic;
using WeirdUnitBE.GameLogic.TowerPackage.Towers;
using WeirdUnitBE.GameLogic.Strategies;

namespace WeirdUnitBE.GameLogic
{
    public class ArrivedToExecutive : IGameStateExecutable
    {
        public object ExecuteCommand(dynamic args, GameState gameState)
        {
            dynamic payload = args.jsonObj.payload;
            Position towerPosition = new Position((int) payload.position.X, (int) payload.position.Y);

            Tower tower = gameState.PositionToTowerDict[towerPosition];
            tower.unitCount -= (int)payload.unitCount;
            //int attackingUnits = (int)payload.unitCount;

            return FormatCommand(tower.unitCount, towerPosition);
        }

        private dynamic FormatCommand(int unitCount, Position position)
        {
            return new
            {
                command = Constants.JsonCommands.ServerCommands.ARRIVED_TO,
                payload = new
                {
                    position = position,
                    unitCount = unitCount
                }
            };
        }
    }
}