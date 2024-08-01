using System;
using System.Collections.Generic;
using VContainer;

namespace Content.Items.Scripts
{
    public class InventoryManager : IInventoryManager
    {
        [Inject] private IReadOnlyList<IItemManager> _injectedItemManagers;

        private Dictionary<Type, IItemManager> _itemManagers;

        public void AddItem<T>(int itemCount) where T : IItem
        {
            var itemType = typeof(T);
        }

        public void RemoveItem<T>(int itemCount) where T : IItem
        {
            var itemType = typeof(T);
        }
        
        [Inject]
        private void Init()
        {
            foreach (var itemManager in _injectedItemManagers)
            {
                _itemManagers[itemManager.GetType()] = itemManager;
            }
        }
    }
}