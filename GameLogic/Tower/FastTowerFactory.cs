using System;
using WeirdUnitBE.GameLogic.Tower;

namespace WeirdUnitBE.GameLogic.Tower
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
