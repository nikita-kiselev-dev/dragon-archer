namespace Core.Items.Scripts
{
    public interface IInventoryManager
    {
        public bool AddItem(string itemName, int itemCount);
        public bool RemoveItem(string itemName, int itemCount);
    }
}