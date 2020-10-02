using System;

namespace WeirdUnitBE.GameLogic.Tower
{
    abstract class AbstractFactory
    {
        public abstract Tower CreateRegeneratingTower();
        public abstract Tower CreateAttackingTower();
    }
}