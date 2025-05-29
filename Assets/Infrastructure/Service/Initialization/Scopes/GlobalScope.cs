using System;
using Content.DailyBonus.Scripts.Data;
using Content.Items.Common.Scripts;
using Content.Items.Common.Scripts.Data;
using Content.Items.Gems.Scripts;
using Content.Items.Gems.Scripts.Data;
using Content.Items.Gold.Scripts;
using Content.Items.Gold.Scripts.Data;
using Content.SettingsPopup.Scripts.Data;
using Infrastructure.Game.Data;
using Infrastructure.Game.Tutorials;
using Infrastructure.Game.Tutorials.Data;
using Infrastructure.Service.Analytics;
using Infrastructure.Service.Analytics.Amplitude;
using Infrastructure.Service.Asset;
using Infrastructure.Service.Asset.IconController;
using Infrastructure.Service.Dto;
using Infrastructure.Service.File;
using Infrastructure.Service.Initialization.Decorators.FastView;
using Infrastructure.Service.LiveOps;
using Infrastructure.Service.LiveOps.GamePush;
using Infrastructure.Service.LiveOps.PlayFab;
using Infrastructure.Service.Logger;
using Infrastructure.Service.SaveLoad;
using Infrastructure.Service.Scene;
using Infrastructure.Service.ScriptableObjects;
using Infrastructure.Service.SignalBus;
using Infrastructure.Service.View.Canvas;
using Infrastructure.Service.View.ViewFactory;
using Infrastructure.Service.View.ViewManager;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.Service.Initialization.Scopes
{
    public class GlobalScope : LifetimeScope
    {
        [SerializeField] private ServiceConfig m_ServiceConfig;
        
        private readonly ILogManager _logger = new LogManager(nameof(GlobalScope));
        
        protected override void Configure(IContainerBuilder builder)
        {
            if (!m_ServiceConfig)
            {
                _logger.LogError($"No ServiceConfig in {nameof(GlobalScope)}.");
                throw new NullReferenceException();
            }
            
            RegisterFileServices(builder);
            RegisterItemsData(builder);
            RegisterFeaturesData(builder);
            RegisterTutorialData(builder);
            RegisterDataManager(builder);
            RegisterAnalytics(builder);
            RegisterLiveOps(builder);
            
            builder.Register<ViewFactory>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<CanvasManager>(Lifetime.Singleton).As<ControlEntity>().AsImplementedInterfaces();
            
            builder.Register<MainAddressableAssetLoader>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<TutorialService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<SceneService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<SceneStateMachine.SceneStateMachine>(Lifetime.Singleton).AsImplementedInterfaces();
            
            builder.Register<EventSignalBus>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<ProgressSaver>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<IconController>(Lifetime.Singleton).AsImplementedInterfaces();
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
                builder.Register<PlayerPrefsSaveLoadService>(Lifetime.Singleton)
                    .As<ControlEntity>()
                    .AsImplementedInterfaces();

                builder.Register<PlayerPrefsDtoManager>(Lifetime.Singleton).AsImplementedInterfaces();
            }
            else if (currentConfig == SaveLoadServices.File)
            {
                builder.Register<ISaveLoadService, FileSaveLoadService>(Lifetime.Singleton).As<IDataSaver>();
                builder.Register<IDtoManager, FileDtoManager>(Lifetime.Singleton).As<IDtoReader>();
            }

            builder.Register<IDataManager, DataManager>(Lifetime.Singleton);
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