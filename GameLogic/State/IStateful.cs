namespace WeirdUnitGame.GameLogic.State
{
    public interface IStateful
    {
        public void TransitionTo(State state);
    }
}