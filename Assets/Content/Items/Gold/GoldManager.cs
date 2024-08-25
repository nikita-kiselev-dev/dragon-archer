using Content.Items.Gold.Data;
using Content.Items.Scripts;
using VContainer;

namespace Content.Items.Gold
{
    public class GoldManager : IItemManager
    {
        [Inject] private readonly GoldData _itemData;

        public string ItemName => ItemsInfo.GoldItemName;

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