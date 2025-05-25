using System.Collections.Generic;
using VContainer;

namespace Content.Items.Common.Scripts
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
            if (!_itemManagers.TryGetValue(itemName, out var manager))
            {
                return false;
            }
            
            var isSuccess = manager.AddItem(itemCount);
            
            return isSuccess;
        }

        public bool RemoveItem(string itemName, int itemCount)
        {
            if (!_itemManagers.TryGetValue(itemName, out var manager))
            {
                return false;
            }
            
            var isSuccess = manager.RemoveItem(itemCount);
            
            return isSuccess;
        }
    }
}