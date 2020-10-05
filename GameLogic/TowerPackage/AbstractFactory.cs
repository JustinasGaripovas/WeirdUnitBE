using System;

namespace WeirdUnitBE.GameLogic.TowerPackage
{
    abstract class AbstractFactory
    {
        public abstract Tower CreateRegeneratingTower();
        public abstract Tower CreateAttackingTower();
    }
}