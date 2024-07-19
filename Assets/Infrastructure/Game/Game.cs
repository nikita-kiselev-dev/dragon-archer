using System;
using System.Collections.Generic;
using Infrastructure.Game.GameManager;
using Infrastructure.Service.StateMachine;
using Infrastructure.Service.StateMachine.SceneStates;
using VContainer;

namespace Infrastructure.Game
{
    public class Game: IGame
    {
        [Inject] private readonly IStateMachine _sceneStateMachine;
        [Inject] private readonly IStartGameManager _startGameManager;
        [Inject] private readonly IMetaGameManager _metaGameManager;
        [Inject] private readonly ICoreGameManager _coreGameManager;
        
        private IGameManager _currentGameManager;
        private Dictionary<Type, IGameManager> _gameManagers;

        public void Init()
        {
            SetupGameManagers();
            SetupSceneStateMachine();
            
            _sceneStateMachine.EnterState<StartSceneState>();
        }

        private void SetupGameManagers()
        {
            _gameManagers = new Dictionary<Type, IGameManager>
            {
                { typeof(StartGameManager), _startGameManager },
                { typeof(MetaGameManager), new MetaGameManager() },
                { typeof(CoreGameManager), new CoreGameManager() }
            };
        }


        private void SetupSceneStateMachine()
        {
            _sceneStateMachine.Init(_gameManagers);
        }
    }
}