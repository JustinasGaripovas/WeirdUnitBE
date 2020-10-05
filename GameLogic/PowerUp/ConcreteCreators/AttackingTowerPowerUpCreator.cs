using System;
using WeirdUnitBE.GameLogic.PowerUp;
using WeirdUnitBE.GameLogic.PowerUp.ConcretePowerUps;

namespace WeirdUnitBE.GameLogic.PowerUp.ConcreteCreators
{
    class AttackingTowerPowerUpCreator: PowerUpCreator
    {
        public override PowerUp createPowerUp()
        {
            return new AttackingTowerPowerUp();
        }
    }
}