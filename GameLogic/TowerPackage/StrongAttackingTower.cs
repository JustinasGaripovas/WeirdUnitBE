using System;

namespace WeirdUnitBE.GameLogic.TowerPackage
{
    class StrongAttackingTower : AttackingTower
    {
        public StrongAttackingTower()
        {
            type = "StrongAttackingTower";
            Console.WriteLine("StrongAttackingTower Created.");
        }
    }
}