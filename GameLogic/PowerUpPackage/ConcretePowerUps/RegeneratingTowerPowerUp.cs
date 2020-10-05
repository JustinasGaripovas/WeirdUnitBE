using System;
using WeirdUnitBE.GameLogic.PowerUpPackage;

namespace WeirdUnitBE.GameLogic.PowerUpPackage.ConcretePowerUps
{
    class RegeneratingTowerPowerUp : PowerUp
    {
        public RegeneratingTowerPowerUp()
        {
            type = "RegeneratingTowerPowerUp";
        }
        public override void printInfo()
        {
            System.Console.WriteLine("RegeneratingTowerPowerUp Created.");
        }     
    }
}