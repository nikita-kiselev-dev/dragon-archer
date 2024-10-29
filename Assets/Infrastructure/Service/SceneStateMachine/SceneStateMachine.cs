using System;
using System.Collections.Generic;
using Infrastructure.Service.Scene;
using Infrastructure.Service.SceneStateMachine.SceneStates;

namespace Infrastructure.Service.SceneStateMachine
{
    public class SceneStateMachine : ISceneStateMachine
    {
        private readonly Dictionary<Type, ISceneState> _states;
        private IState _activeState;
        
        public SceneStateMachine(ISceneService sceneService)
        {
            _states = new Dictionary<Type, ISceneState>
            {
                { typeof(StartSceneState), new StartSceneState(sceneService) },
                { typeof(CoreSceneState), new CoreSceneState(sceneService) },
                { typeof(MetaSceneState), new MetaSceneState(sceneService) }
            };
        }

        public void EnterState<TState>() where TState : IState
        {
            _activeState?.Exit();
            var state = _states[typeof(TState)];
            _activeState = state;
            _activeState?.Enter();
        }
    }
}
