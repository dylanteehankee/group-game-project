namespace Inventory
{
    public interface ItemCollection
    {
        void AddItem(string itemID);

        void RemoveItem(string itemID);

        bool CanAddItem(string itemID);

        bool CanRemoveItem(string itemID);

    }
}