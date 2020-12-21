namespace WeirdUnitGame.GameLogic.State.ConcreteStates
{
    public class CooldownState : State
    {
        public override void Execute()
        {
        }

        public override void Cooldown()
        {
        }

        public override void Ready()
        {
            PowerUp.TransitionTo(new ReadyState());
        }
    }
}