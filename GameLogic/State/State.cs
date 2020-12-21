using WeirdUnitBE.GameLogic.PowerUpPackage;

namespace WeirdUnitGame.GameLogic.State
{
    public abstract class State
    {
        protected PowerUp PowerUp;

        public void SetPowerUp(PowerUp powerUp)
        {
            PowerUp = powerUp;
        }

        public abstract void Execute();
        
        public abstract void Cooldown();
        
        public abstract void Ready();
    }
}