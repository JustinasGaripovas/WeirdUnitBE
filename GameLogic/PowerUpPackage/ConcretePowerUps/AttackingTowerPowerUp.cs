using System;
using WeirdUnitBE.GameLogic.PowerUpPackage;

namespace WeirdUnitBE.GameLogic.PowerUpPackage.ConcretePowerUps
{
    public class AttackingTowerPowerUp : PowerUp
    {
        public AttackingTowerPowerUp()
        {
            type = "AttackingTowerPowerUp";
        }

        public override void printInfo()
        {
            System.Console.WriteLine("AttackingTowerPowerUp Created.");
        }     
    }
}