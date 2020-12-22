using System;
using WeirdUnitBE.GameLogic.PowerUpPackage;
using WeirdUnitBE.GameLogic.TowerPackage.Towers;

namespace WeirdUnitBE.GameLogic.PowerUpPackage.ConcretePowerUps
{
    public class TowerDefencePowerUp : PowerUp
    {
        public TowerDefencePowerUp()
        {
            executionTimeInSeconds = 8;  
            type = "TowerDefencePowerUp";
        }
        public override void printInfo()
        {
            System.Console.WriteLine("TowerDefencePowerUp Created.");
        }
    }
}