using System;

namespace WeirdUnitBE.GameLogic.TowerPackage
{
    [Serializable]
    class StrongAttackingTower : AttackingTower
    {
        public StrongAttackingTower()
        {
            Console.WriteLine("StrongAttackingTower Created.");
        }
    }
}