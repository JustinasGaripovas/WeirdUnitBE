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
        public void ExecuteStrategy(Tower towerFrom, Tower towerTo, int movingUnitCount)
        {
            if (IsAllowedToReinforcePosition(towerTo, towerFrom))
            {
                HandleReinforceLogic(towerFrom, towerTo, movingUnitCount);
            }
        }

        private static void HandleReinforceLogic(Tower towerFrom, Tower towerTo, int movingUnitCount)
        {
            towerTo.unitCount += movingUnitCount;
        }

        private bool IsAllowedToReinforcePosition(Tower towerTo, Tower towerFrom)
        {
            return MapService.GetDefaultMapConnections()[towerFrom.position].ToList().Contains(towerTo.position);
        }
    }
}