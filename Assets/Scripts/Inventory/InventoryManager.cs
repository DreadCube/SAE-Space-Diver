using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public enum SortDirection
    {
        Asc,
        Desc
    }

    public enum SortType
    {
        Amount,
        Hue,
        ShapeType
    }

    private List<InventoryItem> inventoryItems = new List<InventoryItem>();

    private SortType activeSortType = SortType.Amount;
    private SortDirection activeSortDirection = SortDirection.Desc;
    private InventoryItem activeInventoryItem;

    private int defaultInventoryAmount = 5;

    public void SortInventoryItems(SortType sortType, SortDirection sortDirection, bool forceSort = false)
    {
        /*
         * Nothing to do if the requested sorting is the current active sorting
         * and not a forced sort.
         * 
         * A forced sort is happening at the beginning and if we picked up a item.
         */
        if (sortType == activeSortType && activeSortDirection == sortDirection && !forceSort)
        {
            return;
        }

        switch (sortType)
        {
            case SortType.Amount:
                SortAmount.Sort(inventoryItems, sortDirection);
                break;

            case SortType.Hue:
                SortHue.Sort(inventoryItems, sortDirection);
                break;
            default:
                SortShapeType.Sort(inventoryItems, sortDirection);
                break;
        }

        activeSortType = sortType;
        activeSortDirection = sortDirection;
    }

    public List<InventoryItem> GetInventoryItems() => inventoryItems;

    public SortType GetActiveSortType() => activeSortType;

    public SortDirection GetActiveSortDirection() => activeSortDirection;

    public SortDirection GetInactiveSortDirection()
    {
        if (activeSortDirection == SortDirection.Asc)
        {
            return SortDirection.Desc;
        }
        return SortDirection.Asc;
    }

    public InventoryItem GetActiveInventoryItem() => activeInventoryItem;


    /// <summary>
    /// Set active Inventory Item based on InventoryItem
    /// </summary>
    /// <param name="item">inventory item</param>
    public void SetActiveInventoryItem(InventoryItem item)
    {
        activeInventoryItem = inventoryItems.Find(inventoryItem => inventoryItem == item);
    }

    /// <summary>
    /// Set the active inventoryItem based on -1 or 1
    /// </summary>
    /// <example>
    /// <code>
    /// /* Move index one to the right (will go to 0 index if index is > list length) */
    /// SetActiveInventoryItem(1)
    /// </code>
    /// </example>
    /// <example>
    /// <code>
    /// /* Move index one to the left (will go to last index if index is < 0) */
    /// SetActiveInventoryItem(-1)
    /// </code>
    /// </example>
    /// <param name="index">-1 or 1</param>
    public void SetActiveInventoryItem(sbyte index)
    {
        int currentIndex = inventoryItems.FindIndex(inventoryItem => inventoryItem == activeInventoryItem);

        if (index == -1)
        {
            if (currentIndex - 1 >= 0)
            {
                activeInventoryItem = inventoryItems[currentIndex - 1];
                return;
            }
            activeInventoryItem = inventoryItems[inventoryItems.Count - 1];
        }
        if (index == 1)
        {
            if (currentIndex + 1 <= inventoryItems.Count - 1)
            {
                activeInventoryItem = inventoryItems[currentIndex + 1];
                return;
            }
            activeInventoryItem = inventoryItems[0];
        }
    }

    /**
     * Adds a PickupItem to the inventory
     */
    public void AddItem(PickupItem pickupItem)
    {
        // find the corresponding inventory item that matches the provided pickup
        // item and increase it's amount by one.
        InventoryItem inventoryItem = inventoryItems.Find(item => item.GetShape() == pickupItem.GetShape());
        inventoryItem.Increase();

        SortInventoryItems(activeSortType, activeSortDirection, true);

        /**
         * Redraws the inventory UI to show the new amount.
         * TODO: Ideally we do not have to draw the whole inventory UI.
         */
        GameLoopManager.Instance.DrawInventoryUI();
    }

    /**
     * Returns a list of inventory Items that are under the current average.
     * 
     * Average = The current average of all inventory items amount
     * 
     * This will be used for the EnemySpawnManager to spawn these enemy types.
     * The target with that is, that the player will not run out of ammo and
     * balances the gameplay
     */
    public List<InventoryItem> GetLowestHalfAmount()
    {
        double average = inventoryItems.Average(item => item.GetAmount());

        List<InventoryItem> lowestHalf = inventoryItems.FindAll(item => item.GetAmount() <= average);

        return lowestHalf;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        List<Shape> shapes = ShapeDefinition.GetShapeDefinitions();

        /**
         * Prepare / Preload our inventory Items based on the
         * default inventory amount
         */
        foreach (Shape shape in shapes)
        {
            InventoryItem item = new InventoryItem(shape, defaultInventoryAmount);
            inventoryItems.Add(item);
        }

        // Sorts the items initially based on defaults
        SortInventoryItems(activeSortType, activeSortDirection, true);

        activeInventoryItem = inventoryItems[0];
    }
}