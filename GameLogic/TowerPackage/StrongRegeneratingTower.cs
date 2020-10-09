using System;

namespace WeirdUnitBE.GameLogic.TowerPackage
{
    [Serializable]
    class StrongRegeneratingTower : RegeneratingTower
    {
        public StrongRegeneratingTower()
        {
            type = "StrongRegeneratingTower";
            Console.WriteLine("StrongRegeneratingTower Created.");
        }
    }
}