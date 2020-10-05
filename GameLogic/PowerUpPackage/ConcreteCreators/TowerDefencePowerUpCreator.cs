using System;
using WeirdUnitBE.GameLogic.PowerUpPackage;
using WeirdUnitBE.GameLogic.PowerUpPackage.ConcretePowerUps;

namespace WeirdUnitBE.GameLogic.PowerUpPackage.ConcreteCreators
{
    class TowerDefencePowerUpCreator: PowerUpCreator
    {
        public override PowerUp createPowerUp()
        {
            return new TowerDefencePowerUp();
        }
    }
}