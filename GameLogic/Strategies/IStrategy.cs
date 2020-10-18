using System;
using System.Collections;
using System.Collections.Generic;
using WeirdUnitBE.GameLogic;
using WeirdUnitBE.GameLogic.TowerPackage.Towers;

namespace WeirdUnitBE.GameLogic.Strategies
{
    public interface IStrategy
    {
        void ExecuteStrategy(Tower towerFrom, Tower towerTo, Action<Tower, Tower> UpdateGamestate);
    }
}