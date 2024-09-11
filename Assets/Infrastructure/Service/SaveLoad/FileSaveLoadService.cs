using System.IO;
using Infrastructure.Service.File;
using Infrastructure.Service.SaveLoad.Signals;
using Infrastructure.Service.SignalBus;
using Sirenix.Utilities;
using UnityEngine;
using VContainer;
using MemoryPackSerializer = MemoryPack.MemoryPackSerializer;

namespace Infrastructure.Service.SaveLoad
{
    public class FileSaveLoadService : ISaveLoadService, IDataSaver
    {
        [Inject] private readonly IDataManager _dataManager;
        [Inject] private readonly IFileService _fileService;
        [Inject] private readonly ISignalBus _signalBus;
        
        private bool _isInited;

        public void Init()
        {
            CreateDirectory();
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
            _fileService.Save<byte[]>(SaveLoadInfo.SaveFilePath, serializedSave);
            
            Debug.Log($"<color=cyan>{GetType().Name}</color>: save data saved, path: {SaveLoadInfo.SaveFilePath}");
        }

        private void CreateDirectory()
        {
            if (!Directory.Exists(SaveLoadInfo.SaveFileDirectory))
            {
                Directory.CreateDirectory(SaveLoadInfo.SaveFileDirectory);
            }
        }
        
        private void Load()
        {
            _dataManager.PrepareNewData();
            var saveFile = _fileService.Load<byte[]>(SaveLoadInfo.SaveFilePath);
            
            if (saveFile.IsNullOrEmpty())
            {
                Debug.Log($"<color=cyan>{GetType().Name}</color>: save data is empty, nothing to load.");
                return;
            }

            var deserializedSave = MemoryPackSerializer.Deserialize<IDataRepository>(saveFile);
            _dataManager.SetDataRepository(deserializedSave);
            
            Debug.Log($"<color=cyan>{GetType().Name}</color>: save data loaded, path: {SaveLoadInfo.SaveFilePath}.");
        }
    }
}