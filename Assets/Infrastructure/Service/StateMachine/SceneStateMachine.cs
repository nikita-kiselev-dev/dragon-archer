using System;
using System.Collections.Generic;
using Content.LoadingCurtain.Scripts.Controller;
using Infrastructure.Game.GameManager;
using Infrastructure.Service.Scene;
using Infrastructure.Service.StateMachine.SceneStates;
using VContainer;

namespace Infrastructure.Service.StateMachine
{
    public class SceneStateMachine : IStateMachine
    {
        [Inject] private readonly ILoadingCurtainController _loadingCurtainController;
        [Inject] private readonly ISceneService _sceneService;
        
        private Dictionary<Type, ISceneState> _states;
        private IState _activeState;
        
        public void Init(Dictionary<Type, IGameManager> gameManagers)
        {
            _loadingCurtainController.Init();
            
            _states = new Dictionary<Type, ISceneState>
            {
                { typeof(BootstrapSceneState), new BootstrapSceneState(_sceneService, EnterState<StartSceneState>) },
                { typeof(StartSceneState), new StartSceneState(_sceneService, gameManagers[typeof(StartGameManager)]) },
                { typeof(CoreSceneState), new CoreSceneState(_sceneService, gameManagers[typeof(CoreGameManager)]) },
                { typeof(MetaSceneState), new MetaSceneState(_sceneService, gameManagers[typeof(MetaGameManager)]) }
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
