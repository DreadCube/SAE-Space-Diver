public class InventoryItem
{
    private Shape item;
    private int amount;

    public InventoryItem(Shape item, int amount = 0)
    {
        this.item = item;
        this.amount = amount;
    }

    public int GetAmount() => amount;

    public Shape GetShape() => item;
}
