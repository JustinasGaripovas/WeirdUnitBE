using System;
using System.Collections;
using System.Collections.Generic;
using WeirdUnitBE.GameLogic.TowerPackage.Towers;
using WeirdUnitBE.GameLogic.PowerUpPackage;

namespace WeirdUnitBE.GameLogic.Strategies
{
    public class RegeneratingTowerPowerUpStrategy : IPowerUpStrategy
    {
        public void ExecuteStrategy(PowerUp powerUp, List<Tower> towers)
        {
            Console.WriteLine(""+ towers.Count + " Regenerating Towers Powered Up!");           
        }
    }
}