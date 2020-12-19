using WeirdUnitBE.GameLogic.TowerPackage.Towers;

namespace WeirdUnitGame.GameLogic.Visitor.ConcreteVisitors
{
    public class RegeneratingPowerUpVisitor : IVisitor
    {
        public void VisitAttackingTower(AttackingTower attackingTower)
        {
            attackingTower.unitCount += 10;
        }

        public void VisitRegeneratingTower(RegeneratingTower regeneratingTower)
        {
            regeneratingTower.unitCount += 20;
        }
    }
}