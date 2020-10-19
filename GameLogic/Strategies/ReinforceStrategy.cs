using System;
using System.Collections;
using System.Collections.Generic;
using WeirdUnitBE.GameLogic;
using WeirdUnitBE.GameLogic.TowerPackage.Towers;

namespace WeirdUnitBE.GameLogic.Strategies
{
    public class ReinforceStrategy : IMoveStrategy
    {
        public void ExecuteStrategy(Tower towerFrom, Tower towerTo, Action<Tower, Tower> UpdateGamestate)
        {
            int reinforceUnitCount = towerFrom.unitCount / 2;

            towerFrom.unitCount -= reinforceUnitCount;
            towerTo.unitCount += reinforceUnitCount;

            UpdateGamestate(towerFrom, towerTo);
        }
    }
}