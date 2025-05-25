using Infrastructure.Service.SaveLoad;
using MemoryPack;

namespace Content.Items.Common.Scripts.Data
{
    [MemoryPackable]
    public partial class ItemData : Infrastructure.Service.SaveLoad.Data
    {
        [DataProperty] public int ItemCount { get; internal set; }
        
        public override void PrepareNewData()
        {
            ItemCount = 0;
        }

        public bool AddItem(int itemCount)
        {
            ItemCount += itemCount;
            return true;
        }

        public bool RemoveItem(int itemCount)
        {
            var result = ItemCount - itemCount > 0;
            ItemCount = result ? ItemCount - itemCount : 0;
            return true;
        }
    }
}