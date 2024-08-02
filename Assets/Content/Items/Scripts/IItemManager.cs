namespace Content.Items.Scripts
{
    public interface IItemManager
    {
        public string ItemName { get; }
        public bool AddItem(int itemCount);
        public bool RemoveItem(int itemCount);
    }
}