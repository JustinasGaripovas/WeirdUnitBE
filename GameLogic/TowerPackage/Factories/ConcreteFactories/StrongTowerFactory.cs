using System;
using WeirdUnitBE.GameLogic.TowerPackage.Towers;
using WeirdUnitBE.GameLogic.TowerPackage.Towers.StrongTowers;

namespace WeirdUnitBE.GameLogic.TowerPackage.Factories.ConcreteFactories
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