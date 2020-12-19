using System;
using WeirdUnitBE.GameLogic.PowerUpPackage;
using WeirdUnitBE.GameLogic.TowerPackage.Towers;
using WeirdUnitGame.GameLogic.Visitor;
using WeirdUnitGame.GameLogic.Visitor.ConcreteVisitors;

namespace WeirdUnitBE.GameLogic.PowerUpPackage.ConcretePowerUps
{
    public class TowerDefencePowerUp : PowerUp
    {
        private IVisitor _visitor;
        
        public TowerDefencePowerUp()
        {
            _visitor = new TowerDefencePowerUpVisitor();
            type = "TowerDefencePowerUp";
        }
        public override void printInfo()
        {
            System.Console.WriteLine("TowerDefencePowerUp Created.");
        }

        public override TowersCollection ApplyPowerUp(TowersCollection towersCollection)
        {
            foreach (var tower in towersCollection.GetCollection())
            {
                tower.Accept(_visitor);
            }
            
            return towersCollection;        
        }
    }
}