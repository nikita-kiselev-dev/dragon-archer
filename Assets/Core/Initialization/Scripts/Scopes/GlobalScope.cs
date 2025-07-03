using System;
using Core.Analytics;
using Core.Analytics.Amplitude;
using Core.Asset;
using Core.DailyBonus.Scripts.Data;
using Core.Dto;
using Core.File;
using Core.Items.Scripts;
using Core.Items.Scripts.Data;
using Core.LiveOps;
using Core.Logger;
using Core.SaveLoad;
using Core.Scene;
using Core.ScriptableObjects;
using Core.SettingsPopup.Scripts.Data;
using Core.SignalBus;
using Core.Tutorials.Data;
using Core.View.Canvas.Scripts;
using Core.View.ViewFactory;
using Project.Items.Gems.Scripts;
using Project.Items.Gems.Scripts.Data;
using Project.Items.Gold.Scripts;
using Project.Items.Gold.Scripts.Data;
using UnityEngine;
using VContainer;
using VContainer.Unity;

#if GAMEPUSH_ENABLED
using Core.LiveOps.GamePush;
#endif

#if PLAYFAB_ENABLED
using Core.LiveOps.PlayFab;
#endif

namespace Core.Initialization.Scripts.Scopes
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
            //builder.Register<TutorialService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<SceneService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<SceneStateMachine.SceneStateMachine>(Lifetime.Singleton).AsImplementedInterfaces();
            
            builder.Register<EventSignalBus>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<ProgressSaver>(Lifetime.Singleton).AsImplementedInterfaces();
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
            var currentConfig = m_ServiceConfig.SaveLoadService;
            
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
#if PLAYFAB_ENABLED
            builder.Register<PlayFabService>(Lifetime.Singleton)
                .As<IDtoService>()
                .As<IServerConnectionService>()
                .As<IServerTimeService>();
#elif GAMEPUSH_ENABLED
            builder.Register<GamePushService>(Lifetime.Singleton)
                .AsImplementedInterfaces();
#endif
        }
    }
}