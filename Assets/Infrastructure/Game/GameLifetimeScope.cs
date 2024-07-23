using Content.LoadingCurtain.Scripts.Controller;
using Content.SettingsPopup.Scripts.Data;
using Content.SettingsPopup.Scripts.Presenter;
using Content.StartScreen.Scripts.Controller;
using Infrastructure.Game.GameManager;
using Infrastructure.Game.Tutorials;
using Infrastructure.Game.Tutorials.Data;
using Infrastructure.Service;
using Infrastructure.Service.Asset;
using Infrastructure.Service.Data;
using Infrastructure.Service.SaveLoad;
using Infrastructure.Service.Scene;
using Infrastructure.Service.SignalBus;
using Infrastructure.Service.StateMachine;
using Infrastructure.Service.View.Canvas;
using Infrastructure.Service.View.ViewFactory;
using Infrastructure.Service.View.ViewManager;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.Game
{
    public sealed class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private MonoCoroutineRunner m_CoroutineRunner;
        [SerializeField] private ServiceCanvas m_ServiceCanvas;

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);
        }
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent<ICoroutineRunner>(m_CoroutineRunner);
            builder.RegisterComponent(m_ServiceCanvas);
            
            RegisterDataServices(builder);
            RegisterData(builder);
            RegisterTutorialData(builder);
            RegisterDataManager(builder);
            
            builder.Register<ITutorialService, TutorialService>(Lifetime.Singleton);
            builder.Register<IAssetLoader, AddressableAssetLoader>(Lifetime.Singleton);
            builder.Register<IViewFactory, ViewFactory>(Lifetime.Singleton);
            builder.Register<IMainCanvasController, MainCanvasController>(Lifetime.Singleton);
            builder.Register<ISceneService, SceneService>(Lifetime.Singleton);
            builder.Register<IStateMachine, SceneStateMachine>(Lifetime.Singleton);
            builder.Register<ILoadingCurtainController, LoadingCurtainController>(Lifetime.Singleton);
            builder.Register<IStartScreenController, StartScreenController>(Lifetime.Singleton);
            builder.Register<ISettingsPopupPresenter, SettingsPopupPresenter>(Lifetime.Singleton);
            builder.Register<IViewManager, ViewManager>(Lifetime.Singleton);
            builder.Register<ISignalBus, EventSignalBus>(Lifetime.Singleton);
            builder.Register<IGame, Game>(Lifetime.Singleton);
            
            RegisterGameManagers(builder);
            
            builder.RegisterEntryPoint<GameBootstrapper>();
        }

        private void RegisterDataServices(IContainerBuilder builder)
        {
            builder.Register<IFileService, FileService>(Lifetime.Singleton);
            builder.Register<IDataSerializer, JsonDataSerializer>(Lifetime.Singleton);
        }

        private void RegisterData(IContainerBuilder builder)
        {
            builder.Register<Data, SettingsPopupData>(Lifetime.Singleton).AsSelf();
        }

        private void RegisterTutorialData(IContainerBuilder builder)
        {
            builder.Register<Data, OnboardingTutorialData>(Lifetime.Singleton).AsSelf().As<TutorialData>();

        }

        private void RegisterDataManager(IContainerBuilder builder)
        {
            #if UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID
                builder.Register<IDataManager, DesktopAndMobileDataManager>(Lifetime.Singleton);
            #endif
                        
            #if UNITY_WEBGL
                builder.Register<IDataManager, WebDataManager>(Lifetime.Singleton);
            #endif
        }
        
        private void RegisterGameManagers(IContainerBuilder builder)
        {
            builder.Register<IStartGameManager, StartGameManager>(Lifetime.Singleton);
            builder.Register<IMetaGameManager, MetaGameManager>(Lifetime.Singleton);
            builder.Register<ICoreGameManager, CoreGameManager>(Lifetime.Singleton);
        }
    }
}