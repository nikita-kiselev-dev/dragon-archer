using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Infrastructure.Service.Asset;
using Infrastructure.Service.Data;
using Infrastructure.Service.LiveOps;
using Infrastructure.Service.LiveOps.Signals;
using Infrastructure.Service.SignalBus;
using Newtonsoft.Json;
using UnityEngine;
using VContainer;

namespace Infrastructure.Service.Dto
{
    public class DtoManager : IDtoManager, IDtoReader, IDisposable
    {
        [Inject] private readonly IAssetLoader _assetLoader;
        [Inject] private readonly ISignalBus _signalBus;
        [Inject] private readonly IDtoService _dtoService;
        [Inject] private readonly IFileService _fileService;
        
        private Dictionary<string, string> _serverDto;
        private Dictionary<string, string> _dataDto;

        private bool _isInited;
        
        public void Init()
        {
            TryCreateDirectory();
            TryGetDataDto();
            _signalBus.Subscribe<GetLiveOpsDataCompletedSignal>(this, () => _serverDto = _dtoService.GetDto());
            _isInited = true;
        }
        
        public T Read<T>(string configName)
        {
            if (!_isInited)
            {
                throw new ArgumentNullException();
            }
            
            string config;
            var serverConfigExists = IsConfigExists(configName, _serverDto);
            var dataConfigExists = IsConfigExists(configName, _dataDto);
            var areDtosEqual = AreDtosEqual(_serverDto, _dataDto);

            if (serverConfigExists && !areDtosEqual)
            {
                config = _serverDto[configName];
                Save();
            }
            else if (dataConfigExists)
            {
                config = _dataDto[configName];
            }
            else
            {
                config = GetDummyDto(configName);
            }
            
            var result = (T)JsonConvert.DeserializeObject(config, typeof(T));
            return result;
        }
        
        private void Save()
        {
            if (!_isInited || _serverDto == null)
            {
                return;
            }
            
            var stringDto = JsonConvert.SerializeObject(_serverDto);
            _fileService.Save<string>(DtoManagerInfo.ConfigPath, stringDto);
            Debug.Log($"{GetType().Name}: dto config saved, path: {DtoManagerInfo.ConfigPath}");
        }
        
        private void TryCreateDirectory()
        {
            if (!Directory.Exists(DtoManagerInfo.ConfigDirectory))
            {
                Directory.CreateDirectory(DtoManagerInfo.ConfigDirectory);
            }
        }

        private void TryGetDataDto()
        {
            var config = _fileService.Load<string>(DtoManagerInfo.ConfigPath);
            
            if (config == null)
            {
                return;
            }

            var stringConfig = config.ToString();
            
            if (string.IsNullOrEmpty(stringConfig))
            {
                return;
            }
            
            _dataDto = JsonConvert.DeserializeObject<Dictionary<string, string>>(stringConfig);
        }

        private bool IsConfigExists(string configName, Dictionary<string, string> dto)
        {
            var isConfigExists = dto != null && dto.ContainsKey(configName);
            return isConfigExists;
        }
        
        private string GetDummyDto(string configName)
        {
            var config = _assetLoader.LoadAsset<TextAsset>(configName).Result;
            return config.ToString();
        }

        private bool AreDtosEqual(Dictionary<string, string> firstDto, Dictionary<string, string> secondDto)
        {
            if (firstDto == null || secondDto == null)
            {
                return false;
            }
            
            var areEqual = firstDto
                .OrderBy(kv => kv.Key)
                .SequenceEqual(secondDto.OrderBy(kvp => kvp.Key));

            return areEqual;
        }

        void IDisposable.Dispose()
        {
            _signalBus.Unsubscribe<GetLiveOpsDataCompletedSignal>(this);
        }
    }
}