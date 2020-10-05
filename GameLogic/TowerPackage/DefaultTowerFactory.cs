using System;
using WeirdUnitBE.GameLogic.TowerPackage;

namespace WeirdUnitBE.GameLogic.TowerPackage
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

