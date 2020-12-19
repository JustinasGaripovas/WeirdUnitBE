using System;
using WeirdUnitGame.GameLogic.Visitor;

namespace WeirdUnitBE.GameLogic.TowerPackage.Towers
{
    public class AttackingTower : Tower, IVisitableNode
    {
        public override void Accept(IVisitor visitor)
        {
            visitor.VisitAttackingTower(this);
        }
    }
}