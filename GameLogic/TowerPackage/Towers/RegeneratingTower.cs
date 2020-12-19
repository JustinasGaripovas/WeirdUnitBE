using System;
using WeirdUnitGame.GameLogic.Visitor;

namespace WeirdUnitBE.GameLogic.TowerPackage.Towers
{
    public class RegeneratingTower : Tower
    {
        public override void Accept(IVisitor visitor)
        {
            visitor.VisitRegeneratingTower(this);
        }

    }
}