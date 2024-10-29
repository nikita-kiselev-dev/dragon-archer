namespace Infrastructure.Service.SceneStateMachine
{
    public interface IState
    {
        public void Enter();
        public void Exit();
    }
}