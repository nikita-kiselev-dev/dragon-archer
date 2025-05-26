using System.Collections.Generic;
using Infrastructure.Service.Logger;
using VContainer;

namespace Content.Items.Common.Scripts
{
    public class InventoryManager : IInventoryManager
    {
        private readonly Dictionary<string, IItemManager> _itemManagers = new();
        private readonly ILogManager _logger = new LogManager(nameof(InventoryManager));

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
                _logger.LogError($"Can't add item. ItemManager {itemName} does not exist.");
                return false;
            }
            
            var isSuccess = manager.AddItem(itemCount);
            
            return isSuccess;
        }

        public bool RemoveItem(string itemName, int itemCount)
        {
            if (!_itemManagers.TryGetValue(itemName, out var manager))
            {
                _logger.LogError($"Can't remove item. ItemManager {itemName} does not exist.");
                return false;
            }
            
            var isSuccess = manager.RemoveItem(itemCount);
            
            return isSuccess;
        }
    }
}