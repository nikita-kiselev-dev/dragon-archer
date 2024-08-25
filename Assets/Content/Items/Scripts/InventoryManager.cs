using System.Collections.Generic;
using VContainer;

namespace Content.Items.Scripts
{
    public class InventoryManager : IInventoryManager
    {
        private readonly Dictionary<string, IItemManager> _itemManagers = new();

        [Inject]
        private InventoryManager(IReadOnlyList<IItemManager> injectedItemManagers)
        {
            foreach (var itemManager in injectedItemManagers)
            {
                _itemManagers[itemManager.ItemName] = itemManager;
            }
        }

        public bool AddItem(string itemName, int itemCount)
        {
            if (string.IsNullOrEmpty(itemName) && !_itemManagers.ContainsKey(itemName!))
            {
                return false;
            }
            
            var isSuccess = _itemManagers[itemName].AddItem(itemCount);
            return isSuccess;
        }

        public bool RemoveItem(string itemName, int itemCount)
        {
            if (string.IsNullOrEmpty(itemName) && !_itemManagers.ContainsKey(itemName!))
            {
                return false;
            }
            
            var isSuccess = _itemManagers[itemName].RemoveItem(itemCount);
            return isSuccess;
        }
    }
}