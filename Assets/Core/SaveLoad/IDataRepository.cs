using System.Collections.Generic;
using MemoryPack;

namespace Core.SaveLoad
{
    [MemoryPackable]
    [MemoryPackUnion(0, typeof(DataRepository))]
    public partial interface IDataRepository
    {
        public IEnumerable<Data> LoadedData { get; }
    }
}