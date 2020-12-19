using WeirdUnitBE.GameLogic.TowerPackage.Towers;

namespace WeirdUnitGame.GameLogic.Visitor.ConcreteVisitors
{
    public class RegeneratingPowerUpVisitor : IVisitor
    {
        public void VisitAttackingTower(AttackingTower attackingTower)
        {
            throw new System.NotImplementedException();
        }

        public void VisitRegeneratingTower(RegeneratingTower regeneratingTower)
        {
            throw new System.NotImplementedException();
        }
    }
}