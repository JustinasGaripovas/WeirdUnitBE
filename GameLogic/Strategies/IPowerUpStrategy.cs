using System;
using System.Collections;
using System.Collections.Generic;
using WeirdUnitBE.GameLogic.TowerPackage.Towers;
using WeirdUnitBE.GameLogic.PowerUpPackage;

namespace WeirdUnitBE.GameLogic.Strategies
{
    public interface IPowerUpStrategy
    {
        void ExecuteStrategy(PowerUp powerUp, List<Tower> towers, Action<List<Tower>> UpdateGamestate);
    }
}