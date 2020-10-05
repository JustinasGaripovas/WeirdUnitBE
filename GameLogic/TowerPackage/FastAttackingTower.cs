using System;

namespace WeirdUnitBE.GameLogic.TowerPackage
{
    class FastAttackingTower : AttackingTower
    {
        public FastAttackingTower()
        {
            type = "FastAttackingTower";
            System.Console.WriteLine("FastAttackingTower Created.");
        }
    }
}