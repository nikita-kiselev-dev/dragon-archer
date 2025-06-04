using MemoryPack;

namespace Core.Quests.Data
{
    [MemoryPackable]
    public partial class QuestsData : SaveLoad.Data
    {
        public override void PrepareNewData()
        {
            throw new System.NotImplementedException();
        }
    }
}