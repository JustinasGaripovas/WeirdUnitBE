namespace WeirdUnitGame.GameLogic.State.ConcreteStates
{
    public class ExecutingState: State
    {
        public override void Execute()
        {
        }

        public override void Cooldown()
        {
            PowerUp.TransitionTo(new CooldownState());
        }

        public override void Ready()
        {
            PowerUp.TransitionTo(new ReadyState());
        }
    }
}