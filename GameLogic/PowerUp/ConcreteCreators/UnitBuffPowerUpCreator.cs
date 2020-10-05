using System;
using WeirdUnitBE.GameLogic.PowerUp;
using WeirdUnitBE.GameLogic.PowerUp.ConcretePowerUps;

namespace WeirdUnitBE.GameLogic.PowerUp.ConcreteCreators
{
    class UnitBuffPowerUpCreator: PowerUpCreator
    {
        public override PowerUp createPowerUp()
        {
            return new UnitBuffPowerUp();
        }
    }
}