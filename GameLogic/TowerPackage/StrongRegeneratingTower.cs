using System;

namespace WeirdUnitBE.GameLogic.TowerPackage
{
    class StrongRegeneratingTower : RegeneratingTower
    {
        public StrongRegeneratingTower()
        {
            type = "StrongRegeneratingTower";
            Console.WriteLine("StrongRegeneratingTower Created.");
        }
    }
}