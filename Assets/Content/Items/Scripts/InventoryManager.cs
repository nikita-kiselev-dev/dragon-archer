using System.Collections.Generic;
using VContainer;

namespace Content.Items.Scripts
{
    public class InventoryManager : IInventoryManager
    {
        [Inject] private readonly IReadOnlyList<IItemManager> _injectedItemManagers;

        private readonly Dictionary<string, IItemManager> _itemManagers = new();

        public bool AddItem(string itemName, int itemCount)
        {
            if (string.IsNullOrEmpty(itemName) && !_itemManagers.ContainsKey(itemName))
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
        
        [Inject]
        private void Init()
        {
            foreach (var itemManager in _injectedItemManagers)
            {
                _itemManagers[itemManager.ItemName] = itemManager;
            }
        }
    }
}