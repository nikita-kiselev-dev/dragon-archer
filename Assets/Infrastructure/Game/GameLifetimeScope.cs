using Content.LoadingCurtain.Scripts.Controller;
using Content.SettingsPopup.Scripts.Data;
using Content.SettingsPopup.Scripts.Presenter;
using Content.StartScreen.Scripts.Controller;
using Infrastructure.Game.GameManager;
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

            builder.Register<IFileService, FileService>(Lifetime.Singleton);
            builder.Register<IDataSerializer, JsonDataSerializer>(Lifetime.Singleton);
            
            RegisterData(builder);
            
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

        private void RegisterData(IContainerBuilder builder)
        {
            builder.Register<Data, OnboardingTutorialData>(Lifetime.Singleton).AsSelf();
            builder.Register<Data, SettingsPopupData>(Lifetime.Singleton).AsSelf();
            builder.Register<IDataManager, DesktopAndMobileDataManager>(Lifetime.Singleton);
        }
        
        private void RegisterGameManagers(IContainerBuilder builder)
        {
            builder.Register<IStartGameManager, StartGameManager>(Lifetime.Singleton);
            builder.Register<IMetaGameManager, MetaGameManager>(Lifetime.Singleton);
            builder.Register<ICoreGameManager, CoreGameManager>(Lifetime.Singleton);
        }
    }
}