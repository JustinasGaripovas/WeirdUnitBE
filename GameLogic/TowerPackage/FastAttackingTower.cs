using System;

namespace WeirdUnitBE.GameLogic.TowerPackage
{
    [Serializable]
    class FastAttackingTower : AttackingTower
    {
        public FastAttackingTower()
        {
            type = "FastAttackingTower";
            System.Console.WriteLine("FastAttackingTower Created.");
        }
    }
}