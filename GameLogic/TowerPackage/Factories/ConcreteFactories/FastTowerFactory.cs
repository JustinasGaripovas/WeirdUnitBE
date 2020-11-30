using System;
using WeirdUnitBE.GameLogic.TowerPackage.Towers;
using WeirdUnitBE.GameLogic.TowerPackage.Towers.FastTowers;

namespace WeirdUnitBE.GameLogic.TowerPackage.Factories.ConcreteFactories
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
