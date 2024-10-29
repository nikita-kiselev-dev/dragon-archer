namespace Infrastructure.Service.SceneStateMachine
{
    public interface IStateMachine
    {
        public void EnterState<TState>() where TState : IState;
    }
}