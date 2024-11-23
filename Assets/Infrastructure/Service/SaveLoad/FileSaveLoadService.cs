using System.IO;
using Cysharp.Threading.Tasks;
using Infrastructure.Service.File;
using Infrastructure.Service.Initialization;
using Infrastructure.Service.Initialization.InitOrder;
using Infrastructure.Service.Initialization.Scopes;
using Sirenix.Utilities;
using UnityEngine;
using VContainer;
using MemoryPackSerializer = MemoryPack.MemoryPackSerializer;

namespace Infrastructure.Service.SaveLoad
{
    [ControlEntityOrder(nameof(BootstrapScope), (int)BootstrapSceneInitOrder.SaveLoadService)]
    public class FileSaveLoadService : ControlEntity, ISaveLoadService, IDataSaver
    {
        [Inject] private readonly IDataManager _dataManager;
        [Inject] private readonly IFileService _fileService;
        
        private bool _isInited;

        protected override UniTask Init()
        {
            CreateDirectory();
            LoadData();
            _isInited = true;
            return UniTask.CompletedTask;
        }

        public void SaveData()
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
        
        private void LoadData()
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