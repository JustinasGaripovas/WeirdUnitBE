using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WeirdUnitBE.GameLogic;
using WeirdUnitBE.GameLogic.Map;
using WeirdUnitBE.GameLogic.TowerPackage.Towers;

namespace WeirdUnitBE.GameLogic.Strategies
{
    public class ReinforceStrategy : IMoveToStrategy
    {
        public void ExecuteStrategy(Tower towerFrom, Tower towerTo, Action<Tower, Tower> UpdateGamestate)
        {
            if (IsAllowedToAttackPosition(towerTo, towerFrom))
            {
                HandleReinforceLogic(towerFrom, towerTo);
            }
            
            UpdateGamestate(towerFrom, towerTo);
        }

        private static void HandleReinforceLogic(Tower towerFrom, Tower towerTo)
        {
            int reinforceUnitCount = towerFrom.unitCount / 2;
            towerFrom.unitCount -= reinforceUnitCount;
            towerTo.unitCount += reinforceUnitCount;
        }

        private bool IsAllowedToAttackPosition(Tower towerTo, Tower towerFrom)
        {
            return MapService.GetDefaultMapConnections()[towerFrom.position].ToList().Contains(towerTo.position);
        }
    }
}