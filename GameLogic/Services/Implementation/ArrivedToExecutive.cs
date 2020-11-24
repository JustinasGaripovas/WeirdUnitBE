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
            string uuidFrom = payload.uuidFrom;   

            (Tower towerFrom, Tower towerTo) = GetTowersFromJsonArgs(args as Object, gameState);
            Tower towerFromCopy = towerFrom.Clone();
            towerFromCopy.owner = uuidFrom; 

            IMoveToStrategy moveToStrategy = DetermineStrategy(towerFromCopy, towerTo);    

            int movingUnitCount = payload.unitCount; 
            moveToStrategy.ExecuteStrategy(towerFromCopy, towerTo, movingUnitCount);

            return FormatCommand(towerTo.unitCount, towerTo.owner, towerTo.position);
        }

        private (Tower, Tower) GetTowersFromJsonArgs(dynamic args, GameState gameState)
        {    
            (Position positionFrom, Position positionTo) = GetPositionsFromJsonArgs(args as Object);
           
            Tower towerFrom = gameState.GetTowerFromPosition(positionFrom);
            Tower towerTo = gameState.GetTowerFromPosition(positionTo);

            return (towerFrom, towerTo);
        }

        private (Position, Position) GetPositionsFromJsonArgs(dynamic args)
        {
            dynamic payload = args.jsonObj.payload;

            Position positionFrom = new Position((int) payload.position.X, (int) payload.position.Y);
            Position positionTo = new Position((int) payload.towerToPosition.X, (int) payload.towerToPosition.Y);

            return (positionFrom, positionTo);
        }

        private IMoveToStrategy DetermineStrategy(Tower towerFrom, Tower towerTo)
        {   
            IMoveToStrategy strategy;
            if(AreFriendlyTowers(towerFrom, towerTo))
            {
                strategy = new ReinforceStrategy();
            }
            else {
                strategy = new AttackStrategy();                           
            } 

            return strategy;
        }

        private bool AreFriendlyTowers(Tower tower1, Tower tower2)
        {
            return tower1.owner == tower2.owner;
        }

        private dynamic FormatCommand(int unitCount, string owner, Position position)
        {
            return new
            {
                command = Constants.JsonCommands.ServerCommands.ARRIVED_TO,
                payload = new
                {
                    position = position,
                    unitCount = unitCount,
                    owner = owner
                }
            };
        }
    }
}