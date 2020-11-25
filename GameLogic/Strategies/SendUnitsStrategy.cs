using WeirdUnitBE.GameLogic.Map;
using WeirdUnitBE.GameLogic.TowerPackage.Towers;

namespace WeirdUnitBE.GameLogic.Strategies
{
    public class SendUnitsStrategy : IMoveToStrategy
    {
        public void ExecuteStrategy(Tower towerFrom, Tower towerTo, int movingUnitCount)
        {
            SubtractMovingUnitCountFromTower(movingUnitCount, towerFrom);
        }

        private static void SubtractMovingUnitCountFromTower(int movingUnitCount, Tower towerFrom)
        {
            towerFrom.unitCount -= movingUnitCount;
        }
    }
}