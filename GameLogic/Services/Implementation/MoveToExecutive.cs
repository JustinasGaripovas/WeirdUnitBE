using System;
using System.Collections;
using System.Collections.Generic;
using WeirdUnitBE.GameLogic.TowerPackage.Towers;
using WeirdUnitBE.GameLogic.Strategies;

namespace WeirdUnitBE.GameLogic
{
    public class MoveToExecutive : IGameStateExecutable
    {
        public object ExecuteCommand(dynamic args, GameState gameState)
        {
            // GetPayloadFromArgs
            // GetTowersFromJsonArgs
            // ValidateMovementBetweenTowers
            // GetMovingUnitCountFromTower
            // DetermineStrategy
            // ExecuteStrategy
            // CalculateMovementTimeBetweenTwoTowers
            // FormatCommand
            (Tower towerFrom, Tower towerTo) = GetTowersFromJsonArgs(args as Object, gameState);

            ValidateMovementBetweenTowers(towerFrom, towerTo);

            int movingUnitCount = GetMovingUnitCountFromTower(towerFrom);

            IMoveToStrategy strategy = DetermineStrategy(towerFrom, towerTo);
            strategy.ExecuteStrategy(towerFrom, towerTo, movingUnitCount);

            int movementTimeInSeconds = CalculateMovementTimeBetweenTwoTowers(towerFrom, towerTo, GameState.GAME_SPEED);
                       
            string uuidFrom = towerFrom.owner;

            return FormatCommand(movementTimeInSeconds, towerTo.position, towerFrom.position, movingUnitCount, towerFrom.unitCount, uuidFrom);
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
            if(TowersAreNotNeighbours(towerFrom, towerTo))
            {
                throw new InvalidMovementException("Towers are not neighbours");  
            }
            if(MovedToTheSameTower(towerFrom, towerTo))
            {
                throw new InvalidMovementException("Movement to the same tower is invalid");
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
      
        private static dynamic FormatCommand(int timeInSeconds, Position towerToPosition, Position towerFromPosition, int movingUnitCount, int towerFromUnitCount, string uuidFrom)
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
    }
}