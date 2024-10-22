using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cysharp.Threading.Tasks;
using Infrastructure.Service.Asset;
using Infrastructure.Service.File;
using Infrastructure.Service.LiveOps;
using Infrastructure.Service.LiveOps.Signals;
using Infrastructure.Service.Logger;
using Infrastructure.Service.SignalBus;
using Newtonsoft.Json;
using UnityEngine;
using VContainer;

namespace Infrastructure.Service.Dto
{
    public class PlayerPrefsDtoManager : IDtoManager, IDtoReader
    {
        [Inject] private readonly IAssetLoader _assetLoader;
        [Inject] private readonly ISignalBus _signalBus;
        [Inject] private readonly IDtoService _dtoService;
        [Inject] private readonly IFileService _fileService;

        private readonly ILogManager _logger = new LogManager(nameof(PlayerPrefsDtoManager));
        
        private Dictionary<string, string> _serverDto;
        private Dictionary<string, string> _dataDto;

        private bool _isInited;
        
        public void Init()
        {
            TryGetDataDto();
            _signalBus.Subscribe<GetLiveOpsDataCompletedSignal>(this, SetServerDto);
            _isInited = true;
        }
        
        public async UniTask<T> Read<T>(string configName)
        {
            if (!_isInited)
            {
                return default;
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
                config = await GetDummyDto(configName);
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
            PlayerPrefs.SetString(DtoManagerInfo.ConfigName, stringDto);
            PlayerPrefs.Save();

            _logger.Log("Dto config saved in PlayerPrefs.");
        }

        private void TryGetDataDto()
        {
            var config = PlayerPrefs.GetString(DtoManagerInfo.ConfigName);
            
            if (string.IsNullOrEmpty(config))
            {
                return;
            }
            
            _dataDto = JsonConvert.DeserializeObject<Dictionary<string, string>>(config);
            var stringDto = BuildString(_dataDto);
            _logger.Log($"Data config file:\n{stringDto}.");
        }

        private bool IsConfigExists(string configName, IReadOnlyDictionary<string, string> dto)
        {
            var isConfigExists = dto != null && dto.ContainsKey(configName);
            return isConfigExists;
        }
        
        private async UniTask<string> GetDummyDto(string configName)
        {
            var config = await _assetLoader.LoadAssetAsync<TextAsset>(configName);
            _logger.Log($"Dummy config file:\n{config}.");
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

        private void SetServerDto()
        {
            _serverDto = _dtoService.GetDto();
            
            if (_serverDto is null)
            {
                return;
            }
            
            var stringDto = BuildString(_serverDto);
            _logger.Log($"Server config file:\n{stringDto}.");
        }

        private string BuildString(Dictionary<string, string> dto)
        {
            var stringBuilder = new StringBuilder();

            foreach (var kvp in dto)
            {
                stringBuilder.Append($"{kvp.Key}: {kvp.Value}\n");
            }

            var resultString = stringBuilder.ToString();
            stringBuilder.Clear();
            return resultString;
        }
    }
}