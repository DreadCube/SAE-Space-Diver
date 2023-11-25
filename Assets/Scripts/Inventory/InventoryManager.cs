using UnityEngine;
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

    public void SortInventoryItems(SortType sortType, SortDirection sortDirection, bool isInitialSort = false)
    {
        /*
         * Nothing to do if the requested sorting is the current active sorting
         * and not the initial sort
         */
        if (sortType == activeSortType && activeSortDirection == sortDirection && !isInitialSort)
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
        activeInventoryItem = inventoryItems[0];
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


    /**
     * Sets the active inventoryItem based on requested inventory item.
     */
    public void SetActiveInventoryItem(InventoryItem item)
    {
        activeInventoryItem = inventoryItems.Find(inventoryItem => inventoryItem == item);
    }

    /**
     * Sets the active inventoryItem based on -1 or 1
     * 
     * 1: Set the next inventoryItem in the list as active
     *    If we would go over the length of the list we set the first as active
     * 
     * -1: Set the previous inventoryItem in the list as active
     *     If we would go under the length of the list we set the last as active
     */
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


        /**
         * Redraws the inventory UI to show the new amount.
         * TODO: Ideally we do not have to draw the whole inventory UI.
         */
        UIManager.Instance.DrawInventoryUI();
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

        foreach (Shape shape in shapes)
        {
            InventoryItem item = new InventoryItem(shape, Random.Range(0, 100));
            inventoryItems.Add(item);
        }

        // Sorts the items initially based on defaults
        SortInventoryItems(activeSortType, activeSortDirection, true);
    }
}