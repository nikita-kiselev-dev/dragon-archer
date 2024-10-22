using System;
using Infrastructure.Service.File;
using Infrastructure.Service.Logger;
using Infrastructure.Service.SaveLoad.Signals;
using Infrastructure.Service.SignalBus;
using MemoryPack;
using UnityEngine;
using VContainer;

namespace Infrastructure.Service.SaveLoad
{
    public class PlayerPrefsSaveLoadService : ISaveLoadService, IDataSaver
    {
        [Inject] private readonly IDataManager _dataManager;
        [Inject] private readonly IFileService _fileService;
        [Inject] private readonly ISignalBus _signalBus;

        private readonly ILogManager _logger = new LogManager(nameof(PlayerPrefsSaveLoadService));
        
        private bool _isInited;      
        
        public void Init()
        {
            Load();
            _isInited = true;
            _signalBus.Trigger<SaveFileLoadCompletedSignal>();
        }

        public void Save()
        {
            if (!_isInited)
            {
                return;
            }
            
            var serializedSave = MemoryPackSerializer.Serialize(_dataManager.DataRepository);
            var serializedStringSave = Convert.ToBase64String(serializedSave);
            PlayerPrefs.SetString(SaveLoadInfo.SaveFileName, serializedStringSave);
            PlayerPrefs.Save();
            
            _logger.Log("Save data saved in PlayerPrefs.");
        }
        
        private void Load()
        {
            _dataManager.PrepareNewData();
            var saveFile = PlayerPrefs.GetString(SaveLoadInfo.SaveFileName);
            
            if (string.IsNullOrEmpty(saveFile))
            {
                _logger.Log("Save data is empty, nothing to load.");
                return;
            }

            var byteArraySave = Convert.FromBase64String(saveFile);
            var deserializedSave = MemoryPackSerializer.Deserialize<IDataRepository>(byteArraySave);
            _dataManager.SetDataRepository(deserializedSave);
            
            _logger.Log("Save data loaded from PlayerPrefs.");
        }
    }
}