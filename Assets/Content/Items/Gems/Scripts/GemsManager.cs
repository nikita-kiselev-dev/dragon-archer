using Content.Items.Common.Scripts;
using Content.Items.Gems.Scripts.Data;
using VContainer;

namespace Content.Items.Gems.Scripts
{
    public class GemsManager : IItemManager
    {
        [Inject] private readonly GemsData _itemData;
        
        public string ItemName => ItemsInfo.GemsItemName;
        
        public bool AddItem(int itemCount)
        {
            return _itemData.AddItem(itemCount);
        }

        public bool RemoveItem(int itemCount)
        {
            return _itemData.RemoveItem(itemCount);
        }
    }
}