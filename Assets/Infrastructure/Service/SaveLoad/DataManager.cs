using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using VContainer;

namespace Infrastructure.Service.SaveLoad
{
    public class DataManager : IDataManager
    {
        [Inject] private IEnumerable<Data> _injectedDatas;

        private IDataRepository _loadedDataRepository;

        public IDataRepository DataRepository => new DataRepository(_injectedDatas);
        
        public void SetDataRepository(IDataRepository dataRepository)
        {
            _loadedDataRepository = dataRepository;
            SetDataOnLoad();
        }
                
        public void PrepareNewData()
        {
            foreach (var data in _injectedDatas)
            {
                data.PrepareNewData();
            }
        }
        
        private void SetDataOnLoad()
        {
            var dataPairs = _injectedDatas
                .Join(
                    _loadedDataRepository.LoadedData,
                    injectedData => injectedData.Name(),
                    loadedData => loadedData.Name(),
                    (injectedData, loadedData) => new { InjectedData = injectedData, LoadedData = loadedData });

            var fullLoadedDataStringBuilder = new StringBuilder();

            foreach (var dataPair in dataPairs)
            {
                FillWithLoadedData(dataPair.InjectedData, dataPair.LoadedData, fullLoadedDataStringBuilder);
            }

            Debug.Log($"<color=cyan>{GetType().Name}</color> - loaded save file:\n{fullLoadedDataStringBuilder}");
            fullLoadedDataStringBuilder.Clear();
        }
        
        private void FillWithLoadedData(Data injectedData, Data loadedData, StringBuilder fullLoadedDataStringBuilder)
        {
            var type = loadedData.GetType();
            var parentCount = GetParentCount(type);

            for (var index = 0; index < parentCount && type != null; index++)
            {
                var fields = type
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(field => field.IsDefined(typeof(DataProperty), false));

                var propertyInfos = fields.ToList();
                
                foreach (var propertyInfo in propertyInfos)
                {
                    var value = propertyInfo.GetValue(loadedData);
                    propertyInfo.SetValue(injectedData, value);
                    fullLoadedDataStringBuilder.Append($"{loadedData.Name()} - {propertyInfo.Name}: <color=cyan>{value}</color>\n");
                }

                if (propertyInfos.Any())
                {
                    break;
                }

                type = type.BaseType;
            }
        }

        private int GetParentCount(Type type)
        {
            var count = 0;
            var currentType = type.BaseType;

            while (currentType != null)
            {
                count++;
                currentType = currentType.BaseType;
            }

            return count;
        }
    }
}