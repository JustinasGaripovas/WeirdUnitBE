using System;
using WeirdUnitBE.GameLogic.TowerPackage;

namespace WeirdUnitBE.GameLogic.TowerPackage
{
    class FastTowerFactory : AbstractFactory
    {
        public override Tower CreateAttackingTower()
        {
            return new FastAttackingTower();
        }

        public override Tower CreateRegeneratingTower()
        {
            return new FastRegeneratingTower();
        }
    }
}
