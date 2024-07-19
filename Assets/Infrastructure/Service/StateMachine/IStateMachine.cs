using System;
using System.Collections.Generic;
using Infrastructure.Game.GameManager;

namespace Infrastructure.Service.StateMachine
{
    public interface IStateMachine
    {
        public void Init(Dictionary<Type, IGameManager> gameManagers);
        public void EnterState<TState>() where TState : IState;
    }
}