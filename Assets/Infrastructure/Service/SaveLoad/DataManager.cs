using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

            foreach (var dataPair in dataPairs)
            {
                FillWithLoadedData(dataPair.InjectedData, dataPair.LoadedData);
            }
        }
        
        private void FillWithLoadedData(Data injectedData, Data loadedData)
        {
            var type = loadedData.GetType();
            var parentCount = GetParentCount(type);

            for (var index = 0; index < parentCount && type != null; index++)
            {
                var fields = type
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(field => field.IsDefined(typeof(DataProperty), false));

                foreach (var field in fields)
                {
                    var value = field.GetValue(loadedData);
                    field.SetValue(injectedData, value);
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