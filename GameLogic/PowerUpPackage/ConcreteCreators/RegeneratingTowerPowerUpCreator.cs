using System;
using WeirdUnitBE.GameLogic.PowerUpPackage;
using WeirdUnitBE.GameLogic.PowerUpPackage.ConcretePowerUps;

namespace WeirdUnitBE.GameLogic.PowerUpPackage.ConcreteCreators
{
    class RegeneratingTowerPowerUpCreator: PowerUpCreator
    {
        public override PowerUp CreatePowerUp()
        {
            return new RegeneratingTowerPowerUp();
        }
    }
}