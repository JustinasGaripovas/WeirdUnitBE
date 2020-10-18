using System;
using WeirdUnitBE.GameLogic.TowerPackage.Towers;
using WeirdUnitBE.GameLogic.TowerPackage.Towers.DefaultTowers;

namespace WeirdUnitBE.GameLogic.TowerPackage.Factories.ConcreteFactories
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

