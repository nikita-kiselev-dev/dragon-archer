using MemoryPack;

namespace Content.Quests.Data
{
    [MemoryPackable]
    public partial class QuestsData : Infrastructure.Service.SaveLoad.Data
    {
        public override void PrepareNewData()
        {
            throw new System.NotImplementedException();
        }
    }
}