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
    public class SaveLoadService : ISaveLoadService, IDataSaver
    {
        [Inject] private readonly IDataManager _dataManager;
        [Inject] private readonly IFileService _fileService;
        [Inject] private readonly ISignalBus _signalBus;
        
        private bool _isInited;

        public void Init()
        {
            CreateDirectory();
            LoadData();

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
            
            Debug.Log($"{GetType().Name}: save data saved, path: {SaveLoadInfo.SaveFilePath}");
        }

        private void CreateDirectory()
        {
            if (!Directory.Exists(SaveLoadInfo.SaveFileDirectory))
            {
                Directory.CreateDirectory(SaveLoadInfo.SaveFileDirectory);
            }
        }
        
        private void LoadData()
        {
            _dataManager.PrepareNewData();
            var saveFile = _fileService.Load<byte[]>(SaveLoadInfo.SaveFilePath);
            
            if (saveFile.IsNullOrEmpty())
            {
                Debug.Log($"{GetType().Name}: save data is empty, nothing to load.");
                return;
            }

            var deserializedSave = MemoryPackSerializer.Deserialize<IDataRepository>(saveFile);
            _dataManager.SetDataRepository(deserializedSave);
            
            Debug.Log($"{GetType().Name}: save data loaded, path: {SaveLoadInfo.SaveFilePath}.");
        }
    }
}