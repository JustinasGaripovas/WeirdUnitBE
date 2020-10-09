using System;

namespace WeirdUnitBE.GameLogic.TowerPackage
{
    [Serializable]
    class StrongAttackingTower : AttackingTower
    {
        public StrongAttackingTower()
        {
            type = "StrongAttackingTower";
            Console.WriteLine("StrongAttackingTower Created.");
        }
    }
}