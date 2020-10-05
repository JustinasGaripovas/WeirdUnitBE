using System;

namespace WeirdUnitBE.GameLogic.TowerPackage
{
    class FastRegeneratingTower : RegeneratingTower
    {
        public FastRegeneratingTower()
        {
            type = "FastRegeneratingTower";
            System.Console.WriteLine("FastRegeneratingTower Created.");
        }
    }
}