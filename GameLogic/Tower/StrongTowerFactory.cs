using System;
using WeirdUnitBE.GameLogic.Tower;

namespace WeirdUnitBE.GameLogic.Tower
{
    class StrongTowerFactory : AbstractFactory
    {
        public override Tower CreateRegeneratingTower()
        {
            return new StrongRegeneratingTower();
        }

        public override Tower CreateAttackingTower()
        {
            return new StrongAttackingTower();
        }
    }
}