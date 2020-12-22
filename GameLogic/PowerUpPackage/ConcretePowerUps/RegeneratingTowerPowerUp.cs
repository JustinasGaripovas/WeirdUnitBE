using System;
using WeirdUnitBE.GameLogic.PowerUpPackage;
using WeirdUnitBE.GameLogic.TowerPackage.Towers;
using WeirdUnitGame.GameLogic.Visitor;
using WeirdUnitGame.GameLogic.Visitor.ConcreteVisitors;

namespace WeirdUnitBE.GameLogic.PowerUpPackage.ConcretePowerUps
{
    public class RegeneratingTowerPowerUp : PowerUp
    {
        public RegeneratingTowerPowerUp()
        {
            executionTimeInSeconds = 3;  
            type = "RegeneratingTowerPowerUp";
        }
        public override void printInfo()
        {
            System.Console.WriteLine("RegeneratingTowerPowerUp Created.");
        }

    }
}