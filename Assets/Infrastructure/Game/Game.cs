﻿using System;
using Infrastructure.Service.Initialization.Signals;
using Infrastructure.Service.SceneStateMachine;
using Infrastructure.Service.SceneStateMachine.SceneStates;
using Infrastructure.Service.SignalBus;
using VContainer;

namespace Infrastructure.Game
{
    public class Game: IGame, IDisposable
    {
        private readonly IStateMachine _sceneStateMachine;
        private readonly ISignalBus _signalBus;

        [Inject]
        private Game(IStateMachine sceneStateMachine, ISignalBus signalBus)
        {
            _sceneStateMachine = sceneStateMachine;
            _signalBus = signalBus;
            
            Init();
        }

        public void Init()
        {
            //_sceneStateMachine.Init();
            return;
            _signalBus.Subscribe<OnPostInitPhaseCompletedSignal>(this, EnterStartSceneState);
        }

        private void EnterStartSceneState()
        {
            _sceneStateMachine.EnterState<StartSceneState>();
        }

        void IDisposable.Dispose()
        {
            _signalBus.Unsubscribe<OnPostInitPhaseCompletedSignal>(this);
        }
    }
}