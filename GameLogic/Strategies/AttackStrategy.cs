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
        public void ExecuteStrategy(Tower towerFrom, Tower towerTo, Action<Tower, Tower> UpdateGamestate)
        {
            if (IsAllowedToAttackPosition(towerTo, towerFrom))
            {
                HandleAttackingLogic(towerFrom, towerTo);
            }

            UpdateGamestate(towerFrom, towerTo);
        }

        private bool IsAllowedToAttackPosition(Tower towerTo, Tower towerFrom)
        {
            return MapService.GetDefaultMapConnections()[towerFrom.position].ToList().Contains(towerTo.position);
        }

        private static void HandleAttackingLogic(Tower towerFrom, Tower towerTo)
        {
            int attackUnitCount = towerFrom.unitCount / 2;
            towerFrom.unitCount -= attackUnitCount;
            
            if (attackUnitCount > towerTo.unitCount)
            {
                towerTo.owner = towerFrom.owner;
                towerTo.unitCount = Math.Abs(towerTo.unitCount - attackUnitCount);
            }
            else if (attackUnitCount == towerTo.unitCount)
            {
                towerTo.owner = String.Empty;
                towerTo.unitCount = 0;
            }
            else
            {
                towerTo.unitCount -= attackUnitCount;
            }
        }
    }
}