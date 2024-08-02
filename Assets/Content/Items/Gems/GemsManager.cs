using Content.Items.Gems.Data;
using Content.Items.Scripts;
using VContainer;

namespace Content.Items.Gems
{
    public class GemsManager : IItemManager
    {
        [Inject] private readonly GemsData _itemData;
        
        public string ItemName => "gems";
        
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