using System;
using WeirdUnitBE.GameLogic.TowerPackage.Towers;

namespace WeirdUnitBE.GameLogic.TowerPackage.Factories
{
    public abstract class AbstractFactory
    {
        public Tower CreateConcreteTower(string concreteTowerType)
        {
            Tower createdTower;
            switch(concreteTowerType)
            {
                case "Attacking":
                    createdTower = CreateAttackingTower();
                    break;
                case "Regenerating":
                    createdTower = CreateRegeneratingTower();
                    break;
                default:
                    throw new InvalidTowerTypeException("Invalid tower type");
            }

            return createdTower;
        }
        
        public abstract Tower CreateRegeneratingTower();
        public abstract Tower CreateAttackingTower();
    }
}