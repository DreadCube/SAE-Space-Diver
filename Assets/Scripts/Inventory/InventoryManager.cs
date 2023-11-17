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
            InventoryItem item = new InventoryItem((Shape)definitions[i], Random.Range(10, 10));
            inventoryItems.Add(item);
        }
    }

    private void Start()
    {
        SortInventoryItems(activeSortType, activeSortDirection, true);
        UIManager.Instance.Init();
    }

    public void SortInventoryItems(SortType sortType, SortDirection sortDirection, bool isInitialSort = false)
    {
        // Nothing to do if the requested sorting is the current active sorting
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

    public List<InventoryItem> GetInventoryItems()
    {
        return inventoryItems;
    }

    public SortType GetActiveSortType()
    {
        return activeSortType;
    }

    public SortDirection GetActiveSortDirection()
    {
        return activeSortDirection;
    }
}