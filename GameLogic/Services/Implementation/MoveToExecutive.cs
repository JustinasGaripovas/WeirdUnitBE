using System;
using System.Collections;
using System.Collections.Generic;
using WeirdUnitBE.GameLogic.TowerPackage.Towers;
using WeirdUnitBE.GameLogic.Strategies;
using WeirdUnitBE.Middleware.JsonHandling;

namespace WeirdUnitBE.GameLogic
{
    public class MoveToExecutive : IGameStateExecutable
    {
        public object ExecuteCommand(dynamic args, GameState gameState)
        {
            (Tower towerFrom, Tower towerTo) = GetTowersFromJsonArgs(args as Object, gameState);

            ValidateMovementBetweenTowers(towerFrom, towerTo);

            int movingUnitCount = GetMovingUnitCountFromTower(towerFrom);

            IMoveToStrategy strategy = DetermineStrategy(towerFrom, towerTo);
            strategy.ExecuteStrategy(towerFrom, towerTo, movingUnitCount);

            int movementTimeInSeconds = CalculateMovementTimeBetweenTwoTowers(towerFrom, towerTo, gameState.GetGameSpeed());
                       
            string uuidFrom = towerFrom.owner;

            return FormatCommand(towerFrom, towerTo, movementTimeInSeconds, movingUnitCount);
        }

        private static (Tower, Tower) GetTowersFromJsonArgs(dynamic args, GameState gameState)
        {    
            (Position positionFrom, Position positionTo) = GetPositionsFromJsonArgs(args as Object);
           
            Tower towerFrom = gameState.GetTowerFromPosition(positionFrom);
            Tower towerTo = gameState.GetTowerFromPosition(positionTo);

            return (towerFrom, towerTo);
        }

        private static (Position, Position) GetPositionsFromJsonArgs(dynamic args)
        {
            dynamic payload = args.jsonObj.payload;

            Position positionFrom = new Position((int) payload.moveFrom.X, (int) payload.moveFrom.Y);
            Position positionTo = new Position((int) payload.moveTo.X, (int) payload.moveTo.Y);

            return (positionFrom, positionTo);
        }  

        private static void ValidateMovementBetweenTowers(Tower towerFrom, Tower towerTo)
        {
            if(MovedToTheSameTower(towerFrom, towerTo))
            {
                throw new InvalidMovementException("Movement to the same tower is invalid");
            }
            if(TowersAreNotNeighbours(towerFrom, towerTo))
            {
                throw new InvalidMovementException("Towers are not neighbours");  
            }
        }

        private static bool TowersAreNotNeighbours(Tower towerFrom, Tower towerTo)
        {
            return !towerFrom.HasNeighbourTower(towerTo);
        }

        private static bool MovedToTheSameTower(Tower towerFrom, Tower towerTo)
        {
            Position positionFrom = towerFrom.position; 
            Position positionTo = towerTo.position;

            return positionFrom.Equals(positionTo);
        }

        private static int GetMovingUnitCountFromTower(Tower tower)
        {
            int unitCount = tower.unitCount;
            int movingUnits = unitCount / 2;
            return movingUnits;
        }

        private static IMoveToStrategy DetermineStrategy(Tower towerFrom, Tower towerTo)
        {
            return new SendUnitsStrategy();
        }

        private int CalculateMovementTimeBetweenTwoTowers(Tower towerFrom, Tower towerTo, double movementSpeed)
        {
            var distanceBetweenTwoTowers = CalculateDistanceBetweenTwoTowers(towerTo, towerFrom);
            int movementTime = (int)(distanceBetweenTwoTowers / movementSpeed);
            return movementTime;
        }

        private double CalculateDistanceBetweenTwoTowers(Tower towerTo, Tower towerFrom)
        {
            return towerFrom.position.DistanceToPosition(towerTo.position);
        }    
      
        private static dynamic FormatCommand(Tower towerFrom, Tower towerTo, int timeInSeconds, int movingUnitCount)
        {
            JsonMessageFormatterTemplate formatter = new JsonMoveToMessageFormatter();
            var buffer = formatter.FormatJsonBufferFromParams(towerFrom, towerTo, timeInSeconds, movingUnitCount);

            return buffer;    
        }
    }
}