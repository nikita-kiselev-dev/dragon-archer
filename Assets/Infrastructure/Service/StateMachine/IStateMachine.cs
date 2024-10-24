namespace Infrastructure.Service.StateMachine
{
    public interface IStateMachine
    {
        public void Init();
        public void EnterState<TState>() where TState : IState;
    }
}