using System;
using System.Collections;
using System.Collections.Generic;
using WeirdUnitBE.GameLogic.TowerPackage.Towers;
using WeirdUnitBE.GameLogic.Strategies;

namespace WeirdUnitBE.GameLogic
{
    public class MoveToExecutive : IGameStateExecutable, IUndoCommand
    {
        public object ExecuteCommand(dynamic args, GameState gameState)
        {
            (Tower towerFrom, Tower towerTo) = GetTowerParameters(args as Object, gameState);
            towerFrom.unitCount /= 2;

            //TODO: Calculate time to move from one tower to another
            var distance = CalculateDistance(towerTo, towerFrom);

            int timeInSeconds = (int)(distance / GameState.GAME_SPEED);            
            
            //return FormatCommand(timeInSeconds, towerTo.position, (int)towerFrom.unitCount/2);
            return FormatCommand(timeInSeconds, towerTo.position, towerFrom.unitCount);
        }

        private static double CalculateDistance(Tower towerTo, Tower towerFrom)
        {
            return Math.Sqrt(Math.Pow(towerTo.position.Y - towerFrom.position.Y, 2) +
                             Math.Pow(towerTo.position.X - towerFrom.position.X, 2));
        }

        public object UndoCommand(dynamic args, GameState gameState)
        {
            //TODO: needs implementation
            return null;
        }

        private (Tower, Tower) GetTowerParameters(dynamic args, GameState gameState)
        {
            
            dynamic payload = args.jsonObj.payload;
            
            Position positionFrom = new Position((int) payload.moveFrom.X, (int) payload.moveFrom.Y);
            Tower towerFrom = gameState.PositionToTowerDict[positionFrom];

            Position positionTo = new Position((int) payload.moveTo.X, (int) payload.moveTo.Y);
            Tower towerTo = gameState.PositionToTowerDict[positionTo];

            return (towerFrom, towerTo);
        }

        private dynamic FormatCommand(int timeInSeconds, Position towerToPosition, int unitC)
        {
            return new
            {
                command = Constants.JsonCommands.ServerCommands.MOVE_TO,
                payload = new
                {
                    position = towerToPosition,
                    unitCount = unitC,
                    timeToArriveInSeconds = timeInSeconds
                }
            };
        }

        private dynamic FormatUndo(dynamic affectedTowers)
        {
            return new
            {
                command = Constants.JsonCommands.ServerCommands.UNDO_MOVE_TO,
                payload = new { allTowers = affectedTowers }
            };
        }
    }
}