using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Infrastructure.Service.Data;
using Infrastructure.Service.SaveLoad.Signals;
using Infrastructure.Service.SignalBus;
using Newtonsoft.Json;
using UnityEngine;
using VContainer;

namespace Infrastructure.Service.SaveLoad
{
    public class DesktopAndMobileDataManager : IDataManager, IDataSaver
    {
        [Inject] private IEnumerable<Data> _injectedDatas;
        [Inject] private readonly IFileService _fileService;
        [Inject] private readonly ISignalBus _signalBus;
        [Inject] private readonly IDataSerializer _dataSerializer;
        
        private readonly Dictionary<string, Data> _dataRepository = new();
        
        private bool _isInited;

        public void Init()
        {
            TryCreateDirectory();
            TryLoadData();
            PrepareNewData();

            _isInited = true;
            
            _signalBus.Trigger<SaveFileLoadCompletedSignal>();
        }

        public void Save()
        {
            if (!_isInited)
            {
                return;
            }

            var fullSave = _injectedDatas.ToDictionary(data => data.Name());
            var stringFullSave = JsonConvert.SerializeObject(fullSave);
            _fileService.Save<string>(DataManagerInfo.SaveFilePath, stringFullSave);
            Debug.Log($"{GetType().Name}: save data saved, path: {DataManagerInfo.SaveFilePath}");
        }

        private void TryCreateDirectory()
        {
            if (!Directory.Exists(DataManagerInfo.SaveFileDirectory))
            {
                Directory.CreateDirectory(DataManagerInfo.SaveFileDirectory);
            }
        }
        
        private void TryLoadData()
        {
            var saveFile = _fileService.Load<string>(DataManagerInfo.SaveFilePath);
            
            if (saveFile == null)
            {
                return;
            }

            var stringSaveFile = saveFile.ToString();
            
            if (string.IsNullOrEmpty(stringSaveFile))
            {
                return;
            }
            
            var dataNameToType = CreateTypeMapping();
            var loadedData = _dataSerializer.Deserialize(stringSaveFile, dataNameToType);
            Debug.Log($"{GetType().Name}: save data loaded, path: {DataManagerInfo.SaveFilePath}");

            foreach (var kvp in loadedData)
            {
                _dataRepository[kvp.Key] = kvp.Value;
            }

            SetDataOnLoaded();
        }

        private Dictionary<string, Type> CreateTypeMapping()
        {
            var dataNameToType = new Dictionary<string, Type>();
            
            foreach (var data in _injectedDatas)
            {
                dataNameToType[data.Name()] = data.GetType();
            }

            return dataNameToType;
        }
        
        private void SetDataOnLoaded()
        {
            foreach (var data in _injectedDatas)
            {
                if (_dataRepository.TryGetValue(data.Name(), out var loadedData))
                {
                    FillWithLoadedData(data, loadedData);
                }
            }
        }

        private void FillWithLoadedData(Data cleanData, Data loadedData)
        {
            var fields = cleanData
                .GetType()
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(field => field.IsDefined(typeof(SerializeField), false));

            foreach (var field in fields)
            {
                field.SetValue(cleanData, field.GetValue(loadedData));
            }
        }
        
        private void PrepareNewData()
        {
            foreach (var data in _injectedDatas)
            {
                if (_dataRepository.ContainsKey(data.Name()) && _dataRepository[data.Name()] != null)
                {
                    continue;
                }
                
                data.WhenDataIsNew();
                _dataRepository[data.Name()] = data;
            }
        }
    }
}