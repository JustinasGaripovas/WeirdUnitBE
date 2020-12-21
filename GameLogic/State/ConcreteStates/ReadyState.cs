namespace WeirdUnitGame.GameLogic.State.ConcreteStates
{
    public class ReadyState: State
    {
        public override void Execute()
        {
            PowerUp.TransitionTo(new ExecutingState());
        }

        public override void Cooldown()
        {
        }

        public override void Ready()
        {
        }
    }
}