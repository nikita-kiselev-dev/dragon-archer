using MemoryPack;

namespace Core.Quests.Data
{
    [MemoryPackable]
    public partial class QuestData : SaveLoad.Data
    {
        /*[MemoryPackInclude] public bool IsCompleted { get; private set; }*/

        public override void PrepareNewData()
        {
           // IsCompleted = false;
        }
    }
}