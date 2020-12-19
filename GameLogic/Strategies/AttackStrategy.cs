using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WeirdUnitBE.GameLogic;
using WeirdUnitBE.GameLogic.Map;
using WeirdUnitBE.GameLogic.TowerPackage.Towers;

namespace WeirdUnitBE.GameLogic.Strategies
{
    public class AttackStrategy : IMoveToStrategy
    {
        public void ExecuteStrategy(Tower towerFrom, Tower towerTo, int movingUnitCount)
        {
            HandleAttackingLogic(towerFrom, towerTo, movingUnitCount);
        }

        private static void HandleAttackingLogic(Tower towerFrom, Tower towerTo, int movingUnitCount)
        {
            if (movingUnitCount > towerTo.unitCount)
            {
                towerTo.owner = towerFrom.owner;
            }
            else if (movingUnitCount == towerTo.unitCount)
            {
                towerTo.owner = String.Empty;
            }

            var actualCount = Convert.ToInt32(towerTo.unitCount * towerTo.DefenceModifier);
            
            towerTo.unitCount = Math.Abs(actualCount - movingUnitCount);

        }
    }
}