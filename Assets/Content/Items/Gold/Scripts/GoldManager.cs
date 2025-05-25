using Content.Items.Common.Scripts;
using Content.Items.Gold.Scripts.Data;
using VContainer;

namespace Content.Items.Gold.Scripts
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