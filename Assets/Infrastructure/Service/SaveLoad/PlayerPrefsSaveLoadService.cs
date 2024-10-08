﻿using System;
using Infrastructure.Service.File;
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
            
            Debug.Log($"<color=cyan>{GetType().Name}</color>: save data saved in PlayerPrefs.");
        }
        
        private void Load()
        {
            _dataManager.PrepareNewData();
            var saveFile = PlayerPrefs.GetString(SaveLoadInfo.SaveFileName);
            
            if (string.IsNullOrEmpty(saveFile))
            {
                Debug.Log($"<color=cyan>{GetType().Name}</color>: save data is empty, nothing to load.");
                return;
            }

            var byteArraySave = Convert.FromBase64String(saveFile);
            var deserializedSave = MemoryPackSerializer.Deserialize<IDataRepository>(byteArraySave);
            _dataManager.SetDataRepository(deserializedSave);
            
            Debug.Log($"<color=cyan>{GetType().Name}</color>: save data loaded from PlayerPrefs.");
        }
    }
}