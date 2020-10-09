using System;

namespace WeirdUnitBE.GameLogic.TowerPackage
{
    [Serializable]
    class FastRegeneratingTower : RegeneratingTower
    {
        public FastRegeneratingTower()
        {
            type = "FastRegeneratingTower";
            System.Console.WriteLine("FastRegeneratingTower Created.");
        }
    }
}