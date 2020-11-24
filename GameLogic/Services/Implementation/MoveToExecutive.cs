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
            (Position positionFrom, Position positionTo) = GetPositionsFromJsonArgs(args as Object);

            if(positionFrom.Equals(positionTo))
            {
                throw new InvalidMovementException("Movement to the same tower is invalid");
            }

            Tower towerFrom = gameState.GetTowerFromPosition(positionFrom);
            Tower towerTo = gameState.GetTowerFromPosition(positionTo);

            int movingUnitCount = GetMovingUnitCountFromTower(towerFrom);
            SubtractMovingUnitCountFromTower(movingUnitCount, towerFrom);
            int movementTimeInSeconds = CalculateMovementTimeBetweenTwoTowers(towerFrom, towerTo, GameState.GAME_SPEED);
                       
            string uuidFrom = towerFrom.owner;

            return FormatCommand(movementTimeInSeconds, towerTo.position, towerFrom.position, movingUnitCount, towerFrom.unitCount, uuidFrom);
        }

        private static (Position, Position) GetPositionsFromJsonArgs(dynamic args)
        {
            dynamic payload = args.jsonObj.payload;

            Position positionFrom = new Position((int) payload.moveFrom.X, (int) payload.moveFrom.Y);
            Position positionTo = new Position((int) payload.moveTo.X, (int) payload.moveTo.Y);

            return (positionFrom, positionTo);
        }  

        private static int GetMovingUnitCountFromTower(Tower tower)
        {
            int unitCount = tower.unitCount;
            int movingUnits = unitCount / 2;
            return movingUnits;
        }

        private static void SubtractMovingUnitCountFromTower(int movingUnitCount, Tower tower)
        {
            tower.unitCount -= movingUnitCount;
        }

        private static int CalculateMovementTimeBetweenTwoTowers(Tower towerFrom, Tower towerTo, double movementSpeed)
        {
            var distanceBetweenTwoTowers = CalculateDistanceBetweenTwoTowers(towerTo, towerFrom);
            int movementTime = (int)(distanceBetweenTwoTowers / movementSpeed);
            return movementTime;
        }

        private static double CalculateDistanceBetweenTwoTowers(Tower towerTo, Tower towerFrom)
        {
            return towerFrom.position.DistanceToPosition(towerTo.position);
        }    
      
        private dynamic FormatCommand(int timeInSeconds, Position towerToPosition, Position towerFromPosition, int movingUnitCount, int towerFromUnitCount, string uuidFrom)
        {
            return new
            {
                command = Constants.JsonCommands.ServerCommands.MOVE_TO,
                payload = new
                {
                    towerToPosition = towerToPosition,
                    towerFromPosition = towerFromPosition,
                    unitCount = movingUnitCount,
                    uuidFrom = uuidFrom,
                    timeToArriveInSeconds = timeInSeconds,
                    towerFromUnitCount = towerFromUnitCount
                }
            };
        }
        
        #region Not Implemented Yet
        private dynamic FormatUndo(dynamic affectedTowers)
        {
            return new
            {
                command = Constants.JsonCommands.ServerCommands.UNDO_MOVE_TO,
                payload = new { allTowers = affectedTowers }
            };
        }

        public dynamic UndoCommand(dynamic kazkas, GameState smth)
        {
            return null;
        }
        #endregion
    }
}