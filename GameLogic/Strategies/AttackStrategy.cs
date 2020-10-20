using System;
using System.Collections;
using System.Collections.Generic;
using WeirdUnitBE.GameLogic;
using WeirdUnitBE.GameLogic.TowerPackage.Towers;

namespace WeirdUnitBE.GameLogic.Strategies
{
    public class AttackStrategy : IMoveToStrategy
    {
        public void ExecuteStrategy(Tower towerFrom, Tower towerTo, Action<Tower, Tower> UpdateGamestate)
        {
            int attackUnitCount = towerFrom.unitCount / 2;

            towerFrom.unitCount -= attackUnitCount;

            if(attackUnitCount > towerTo.unitCount)
            {
                towerTo.owner = towerFrom.owner;
                towerTo.unitCount = Math.Abs(towerTo.unitCount - attackUnitCount);
            }
            else if(attackUnitCount == towerTo.unitCount)
            {
                towerTo.owner = String.Empty;
                towerTo.unitCount = 0;
            }
            else
            {
                towerTo.unitCount -= attackUnitCount;
            }

            UpdateGamestate(towerFrom, towerTo);
        }
    }
}