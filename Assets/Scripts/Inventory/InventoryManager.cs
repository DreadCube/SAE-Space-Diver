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
                InsertionSort.Sort(inventoryItems, sortDirection);
                break;

            case SortType.Hue:
                QuickSort.Sort(inventoryItems, sortDirection);
                break;
            default:
                MergeSort.Sort(inventoryItems, sortDirection);
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

        object[] definitions = Resources.LoadAll("ShapeDefinitions", typeof(Shape));

        for (int i = 0; i < definitions.Length; i++)
        {
            InventoryItem item = new InventoryItem((Shape)definitions[i], Random.Range(1, 10));
            inventoryItems.Add(item);
        }

        // Sorts the items initially based on defaults
        SortInventoryItems(activeSortType, activeSortDirection, true);

    }
}