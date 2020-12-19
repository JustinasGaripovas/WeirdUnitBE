using WeirdUnitBE.GameLogic.TowerPackage.Towers;

namespace WeirdUnitGame.GameLogic.Visitor.ConcreteVisitors
{
    public class TowerDefencePowerUpVisitor : IVisitor
    {
        public void VisitAttackingTower(AttackingTower attackingTower)
        {
            attackingTower.DefenceModifier = 1.5f;
        }

        public void VisitRegeneratingTower(RegeneratingTower regeneratingTower)
        {
            regeneratingTower.unitCount += 5;
            regeneratingTower.DefenceModifier = 1.1f;
        }
    }
}