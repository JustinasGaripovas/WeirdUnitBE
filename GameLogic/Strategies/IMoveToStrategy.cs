using System;
using System.Collections;
using System.Collections.Generic;
using WeirdUnitBE.GameLogic;
using WeirdUnitBE.GameLogic.TowerPackage.Towers;

namespace WeirdUnitBE.GameLogic.Strategies
{
    public interface IMoveToStrategy
    {
        void ExecuteStrategy(Tower towerFrom, Tower towerTo, int movingUnitCount);
    }
}