using System;
using WeirdUnitBE.GameLogic.TowerPackage.Towers;

namespace WeirdUnitBE.GameLogic.TowerPackage.Factories
{
    public abstract class AbstractFactory
    {
        public abstract Tower CreateRegeneratingTower();
        public abstract Tower CreateAttackingTower();
    }
}