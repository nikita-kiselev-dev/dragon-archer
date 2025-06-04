using System.Collections.Generic;
using MemoryPack;

namespace Core.SaveLoad
{
    [MemoryPackable]
    public partial class DataRepository : IDataRepository
    {
        [MemoryPackAllowSerialize] public IEnumerable<Data> LoadedData { get; }

        public DataRepository(IEnumerable<Data> loadedData)
        {
            LoadedData = loadedData;
        }
    }
}