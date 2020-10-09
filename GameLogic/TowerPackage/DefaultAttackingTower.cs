using System;

namespace WeirdUnitBE.GameLogic.TowerPackage
{
    [Serializable]
    class DefaultAttackingTower : AttackingTower
    {
        public DefaultAttackingTower()
        {
            type = "DefaultAttackingTower";
            Console.WriteLine("DefaultAttackingTower Created.");
        }     
    }
}