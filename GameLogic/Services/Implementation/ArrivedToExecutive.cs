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

            //Position towerFromPosition = new Position((int) payload.position.X, (int) payload.position.Y);       
            Tower tower = gameState.PositionToTowerDict[towerPosition];                
            string uuidFrom = payload.uuidFrom;
            int movingUnitCount = payload.unitCount;
            if(tower.owner == uuidFrom)
            {
                tower.unitCount += movingUnitCount;
            }
            else {
                if(movingUnitCount > tower.unitCount)
                {
                    tower.owner = uuidFrom;
                }
                else if(movingUnitCount == tower.unitCount)
                {
                    tower.owner = "";
                }               
                tower.unitCount = Math.Abs(tower.unitCount - movingUnitCount);               
            }    

            return FormatCommand(tower.unitCount, tower.owner, towerPosition);
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