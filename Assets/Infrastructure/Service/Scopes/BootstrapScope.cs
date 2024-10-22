using System;
using Content.DailyBonus.Scripts;
using Content.DailyBonus.Scripts.Data;
using Content.Items.Gems;
using Content.Items.Gems.Data;
using Content.Items.Gold;
using Content.Items.Gold.Data;
using Content.Items.Scripts;
using Content.Items.Scripts.Data;
using Content.LoadingCurtain.Scripts.Controller;
using Content.LoadingCurtain.Scripts.View;
using Content.Settings.Scripts;
using Content.Settings.Scripts.Data;
using Content.StartScreen.Scripts.Controller;
using Infrastructure.Game;
using Infrastructure.Game.Data;
using Infrastructure.Game.GameManager;
using Infrastructure.Game.Tutorials;
using Infrastructure.Game.Tutorials.Data;
using Infrastructure.Service.Analytics;
using Infrastructure.Service.Analytics.Amplitude;
using Infrastructure.Service.Asset;
using Infrastructure.Service.Dto;
using Infrastructure.Service.File;
using Infrastructure.Service.Initialization;
using Infrastructure.Service.LiveOps;
using Infrastructure.Service.LiveOps.GamePush;
using Infrastructure.Service.LiveOps.PlayFab;
using Infrastructure.Service.SaveLoad;
using Infrastructure.Service.Scene;
using Infrastructure.Service.ScriptableObjects;
using Infrastructure.Service.SignalBus;
using Infrastructure.Service.StateMachine;
using Infrastructure.Service.View.Canvas;
using Infrastructure.Service.View.ViewFactory;
using Infrastructure.Service.View.ViewManager;
using Infrastructure.Service.View.ViewManager.ViewAnimation;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.Service.Scopes
{
    public sealed class BootstrapScope : LifetimeScope
    {
        [SerializeField] private LoadingCurtainView m_LoadingCurtainView;
        [SerializeField] private ServiceConfig m_ServiceConfig;
        
        protected override void Configure(IContainerBuilder builder)
        {
            if (!m_ServiceConfig)
            {
                throw new NullReferenceException($"No ServiceConfig in BootstrapScope!");
            }

            builder.RegisterComponent(m_LoadingCurtainView).AsImplementedInterfaces();
            
            RegisterFileServices(builder);
            RegisterItemsData(builder);
            RegisterFeaturesData(builder);
            RegisterTutorialData(builder);
            RegisterDataManager(builder);
            RegisterAssetLoader(builder);
            
            builder.RegisterEntryPoint<GameBootstrapper>();
            builder.RegisterEntryPoint<SceneStarter>();
            builder.Register<ITutorialService, TutorialService>(Lifetime.Singleton);
            builder.Register<ViewFactory>(Lifetime.Singleton).As<ControlEntity>().AsImplementedInterfaces();
            builder.Register<IMainCanvasController, MainCanvasController>(Lifetime.Singleton);
            builder.Register<IViewAnimator, BackgroundAnimator>(Lifetime.Singleton);
            builder.Register<ISceneService, SceneService>(Lifetime.Singleton);
            builder.Register<IStateMachine, SceneStateMachine>(Lifetime.Singleton);
            
            RegisterAnalytics(builder);
            RegisterLiveOps(builder);
            
            builder.Register<LoadingCurtainController>(Lifetime.Singleton).As<ControlEntity>().AsImplementedInterfaces();
            builder.Register<StartScreenController>(Lifetime.Singleton).As<ControlEntity>().AsImplementedInterfaces();
            builder.Register<SettingsPopup>(Lifetime.Singleton).As<ControlEntity>().AsImplementedInterfaces();
            builder.Register<ViewManager>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<ISignalBus, EventSignalBus>(Lifetime.Singleton);
            builder.Register<IGame, Game.Game>(Lifetime.Singleton);
            
            RegisterFeatures(builder);
            RegisterGameManagers(builder);
        }

        private void RegisterFileServices(IContainerBuilder builder)
        {
            builder.Register<IFileService, FileService>(Lifetime.Singleton);
        }
        
        private void RegisterItemsData(IContainerBuilder builder)
        {
            builder.Register<Data, GoldData>(Lifetime.Singleton).As<ItemData>().AsSelf();
            builder.Register<IItemManager, GoldManager>(Lifetime.Singleton);
            
            builder.Register<Data, GemsData>(Lifetime.Singleton).As<ItemData>().AsSelf();
            builder.Register<IItemManager, GemsManager>(Lifetime.Singleton);
            
            builder.Register<IInventoryManager, InventoryManager>(Lifetime.Singleton);
        }

        private void RegisterFeaturesData(IContainerBuilder builder)
        {
            builder.Register<IMainDataManager, MainDataManager>(Lifetime.Singleton);
            builder.Register<Data, MainData>(Lifetime.Singleton).AsSelf();
            builder.Register<Data, SettingsPopupData>(Lifetime.Singleton).AsSelf();
            builder.Register<Data, DailyBonusData>(Lifetime.Singleton).AsSelf();
        }

        private void RegisterTutorialData(IContainerBuilder builder)
        {
            builder.Register<Data, OnboardingTutorialData>(Lifetime.Singleton).AsSelf().As<TutorialData>();
        }

        private void RegisterDataManager(IContainerBuilder builder)
        {
            var currentConfig = m_ServiceConfig.m_SaveLoadService;
            
            if (currentConfig == SaveLoadServices.PlayerPrefs)
            {
                builder.Register<ISaveLoadService, PlayerPrefsSaveLoadService>(Lifetime.Singleton).As<IDataSaver>();
                builder.Register<IDtoManager, PlayerPrefsDtoManager>(Lifetime.Singleton).As<IDtoReader>();
            }
            else if (currentConfig == SaveLoadServices.File)
            {
                builder.Register<ISaveLoadService, FileSaveLoadService>(Lifetime.Singleton).As<IDataSaver>();
                builder.Register<IDtoManager, FileDtoManager>(Lifetime.Singleton).As<IDtoReader>();
            }

            builder.Register<IDataManager, DataManager>(Lifetime.Singleton);
        }

        private void RegisterAssetLoader(IContainerBuilder builder)
        {
            builder.Register<MainAddressableAssetLoader>(Lifetime.Singleton).AsImplementedInterfaces();
        }

        private void RegisterFeatures(IContainerBuilder builder)
        {
            builder.Register<IDailyBonus, DailyBonus>(Lifetime.Singleton);
        }
        
        private void RegisterGameManagers(IContainerBuilder builder)
        {
            builder.Register<IStartGameManager, StartGameManager>(Lifetime.Singleton);
            builder.Register<IMetaGameManager, MetaGameManager>(Lifetime.Singleton);
            builder.Register<ICoreGameManager, CoreGameManager>(Lifetime.Singleton);
        }

        private void RegisterAnalytics(IContainerBuilder builder)
        {
            builder.Register<IAnalyticsService, AmplitudeAnalytics>(Lifetime.Singleton);
            builder.Register<IAnalyticsManager, AnalyticsManager>(Lifetime.Singleton);
        }

        private void RegisterLiveOps(IContainerBuilder builder)
        {
            var currentConfig = m_ServiceConfig.m_LiveOpsService;
            
            if (currentConfig == LiveOpsServices.PlayFab)
            {
                builder.Register<PlayFabService>(Lifetime.Singleton)
                    .As<ILiveOpsController>()
                    .As<IDtoService>()
                    .As<IServerConnectionService>()
                    .As<IServerTimeService>();
            }
            else if (currentConfig == LiveOpsServices.GamePush)
            {
                builder.Register<GamePushService>(Lifetime.Singleton)
                    .AsImplementedInterfaces();
            }
        }
    }
}