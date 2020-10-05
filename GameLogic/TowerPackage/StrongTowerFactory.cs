using System;
using WeirdUnitBE.GameLogic.TowerPackage;

namespace WeirdUnitBE.GameLogic.TowerPackage
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