using System;

namespace WeirdUnitBE.GameLogic.TowerPackage
{
    class DefaultAttackingTower : AttackingTower
    {
        public DefaultAttackingTower()
        {
            type = "DefaultAttackingTower";
            Console.WriteLine("DefaultAttackingTower Created.");
        }     
    }
}