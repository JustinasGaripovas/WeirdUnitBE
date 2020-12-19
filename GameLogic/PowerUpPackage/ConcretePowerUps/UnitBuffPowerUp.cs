using System;
using WeirdUnitBE.GameLogic.PowerUpPackage;

namespace WeirdUnitBE.GameLogic.PowerUpPackage.ConcretePowerUps
{
    public class UnitBuffPowerUp : PowerUp
    {
        public override void printInfo()
        {
            System.Console.WriteLine("UnitBuffPowerUp Created.");
        }

        public override TowersCollection ApplyPowerUp(TowersCollection towersCollection)
        {
            return towersCollection;
        }
    }
}