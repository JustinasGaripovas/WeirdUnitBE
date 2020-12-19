using System;
using WeirdUnitBE.GameLogic.PowerUpPackage;
using WeirdUnitBE.GameLogic.TowerPackage.Towers;
using WeirdUnitGame.GameLogic.Visitor;
using WeirdUnitGame.GameLogic.Visitor.ConcreteVisitors;

namespace WeirdUnitBE.GameLogic.PowerUpPackage.ConcretePowerUps
{
    public class RegeneratingTowerPowerUp : PowerUp
    {
        private IVisitor _visitor;
        
        public RegeneratingTowerPowerUp()
        {
            _visitor = new RegeneratingPowerUpVisitor();
            type = "RegeneratingTowerPowerUp";
        }
        public override void printInfo()
        {
            System.Console.WriteLine("RegeneratingTowerPowerUp Created.");
        }
        
        public override TowersCollection ApplyPowerUp(TowersCollection towersCollection)
        {
            foreach (Tower tower in towersCollection.GetCollection())
            {
                tower.Accept(_visitor);
            }
            
            return towersCollection;
        }
    }
}