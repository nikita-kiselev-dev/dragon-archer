using MemoryPack;

namespace Content.Quests.Data
{
    [MemoryPackable]
    public partial class QuestData : Infrastructure.Service.SaveLoad.Data
    {
        /*[MemoryPackInclude] public bool IsCompleted { get; private set; }*/

        public override void PrepareNewData()
        {
           // IsCompleted = false;
        }
    }
}