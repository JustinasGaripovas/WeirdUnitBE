using System;
using WeirdUnitBE.GameLogic.Tower;

namespace WeirdUnitBE.GameLogic.Tower
{
    class DefaultTowerFactory : AbstractFactory
    {
        public override Tower CreateAttackingTower()
        {
            return new DefaultAttackingTower();
        }
        public override Tower CreateRegeneratingTower()
        {
            return new DefaultRegeneratingTower();
        }
    }
}

