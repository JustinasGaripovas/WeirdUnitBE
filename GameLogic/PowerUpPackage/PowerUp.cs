
namespace WeirdUnitBE.GameLogic.PowerUpPackage
{
    public abstract class PowerUp
    {
        public string type;
        public string owner;
        public abstract void printInfo();
        public abstract TowersCollection ApplyPowerUp(TowersCollection towersCollection);
    }
}