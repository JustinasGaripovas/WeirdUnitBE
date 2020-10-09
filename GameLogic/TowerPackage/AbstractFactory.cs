using System;

namespace WeirdUnitBE.GameLogic.TowerPackage
{
    public abstract class AbstractFactory
    {
        public abstract Tower CreateRegeneratingTower();
        public abstract Tower CreateAttackingTower();
    }
}