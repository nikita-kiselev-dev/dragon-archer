using System;
using System.Collections.Generic;
using Infrastructure.Service.Scene;
using Infrastructure.Service.StateMachine.SceneStates;
using VContainer;

namespace Infrastructure.Service.StateMachine
{
    public class SceneStateMachine : IStateMachine
    {
        [Inject] private readonly ISceneService _sceneService;
        
        private Dictionary<Type, ISceneState> _states;
        private IState _activeState;
        
        public void Init()
        {
            _states = new Dictionary<Type, ISceneState>
            {
                { typeof(BootstrapSceneState), new BootstrapSceneState(_sceneService, EnterState<StartSceneState>) },
                { typeof(StartSceneState), new StartSceneState(_sceneService) },
                { typeof(CoreSceneState), new CoreSceneState(_sceneService) },
                { typeof(MetaSceneState), new MetaSceneState(_sceneService) }
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
