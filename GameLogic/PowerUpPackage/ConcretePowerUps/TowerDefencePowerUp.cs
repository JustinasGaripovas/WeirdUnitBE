using System;
using WeirdUnitBE.GameLogic.PowerUpPackage;

namespace WeirdUnitBE.GameLogic.PowerUpPackage.ConcretePowerUps
{
    public class TowerDefencePowerUp : PowerUp
    {
        public TowerDefencePowerUp()
        {
            type = "TowerDefencePowerUp";
        }
        public override void printInfo()
        {
            System.Console.WriteLine("TowerDefencePowerUp Created.");
        }     
    }
}