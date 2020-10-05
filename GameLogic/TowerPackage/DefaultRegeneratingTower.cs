using System;

namespace WeirdUnitBE.GameLogic.TowerPackage
{
    class DefaultRegeneratingTower : RegeneratingTower
    {
        public DefaultRegeneratingTower()
        {
            type = "DefaultRegeneratingTower";
            System.Console.WriteLine("DefaultRegeneratingTower Created.");
        }
    }
}