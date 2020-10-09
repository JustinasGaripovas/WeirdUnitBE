using System;

namespace WeirdUnitBE.GameLogic.TowerPackage
{
    [Serializable]
    class StrongRegeneratingTower : RegeneratingTower
    {
        public StrongRegeneratingTower()
        {
            Console.WriteLine("StrongRegeneratingTower Created.");
        }
    }
}