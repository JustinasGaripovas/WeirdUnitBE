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
            string uuidFrom = payload.uuidFrom;
            int movingUnitCount = payload.unitCount;
            
            Position towerToPosition = new Position((int) payload.towerToPosition.X, (int) payload.towerToPosition.Y);
            Tower towerTo = gameState.PositionToTowerDict[towerToPosition];  
            
            if(towerTo.owner == uuidFrom)
            {
                towerTo.unitCount += movingUnitCount;
            }
            else {
                if(movingUnitCount > towerTo.unitCount)
                {
                    towerTo.owner = uuidFrom;
                }
                else if(movingUnitCount == towerTo.unitCount)
                {
                    towerTo.owner = "";
                }               
                towerTo.unitCount = Math.Abs(towerTo.unitCount - movingUnitCount);               
            }    

            Console.WriteLine(towerTo.unitCount + " cia yra jksdahfkjshfi");

            return FormatCommand(towerTo.unitCount, towerTo.owner, towerToPosition);
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