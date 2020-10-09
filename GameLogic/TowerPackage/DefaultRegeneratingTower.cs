using System;

namespace WeirdUnitBE.GameLogic.TowerPackage
{
    [Serializable]
    class DefaultRegeneratingTower : RegeneratingTower
    {
        public DefaultRegeneratingTower()
        {
            type = "DefaultRegeneratingTower";
            System.Console.WriteLine("DefaultRegeneratingTower Created.");
        }
    }
}