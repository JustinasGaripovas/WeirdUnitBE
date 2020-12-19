using WeirdUnitBE.GameLogic.TowerPackage.Towers;

namespace WeirdUnitGame.GameLogic.Visitor
{
    public interface IVisitor
    {
        void VisitAttackingTower(AttackingTower attackingTower);
        void VisitRegeneratingTower(RegeneratingTower regeneratingTower);
    }
}