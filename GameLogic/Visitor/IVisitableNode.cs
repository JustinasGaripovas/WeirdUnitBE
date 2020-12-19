namespace WeirdUnitGame.GameLogic.Visitor
{
    public interface IVisitableNode
    {
        public void Accept(IVisitor visitor);
    }
}