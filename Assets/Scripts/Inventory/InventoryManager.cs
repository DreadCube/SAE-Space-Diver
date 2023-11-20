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

        List<Shape> shapes = LoadShapeDefinitions();

        foreach (Shape shape in shapes)
        {
            InventoryItem item = new InventoryItem(shape, Random.Range(0, 100));
            inventoryItems.Add(item);
        }

        // Sorts the items initially based on defaults
        SortInventoryItems(activeSortType, activeSortDirection, true);
    }

    /**
     * Loads our available Shape Definitions on runtime
     * 
     * TODO: This logic will be moved later on to a more central place, because we will need the available
     * Shape Definitions in different places (inventory, enemies spawning, pickup items). Should also be potential
     * cached
     */
    private List<Shape> LoadShapeDefinitions()
    {
        object[] definitions = Resources.LoadAll("ShapeDefinitions", typeof(Shape));

        List<Shape> shapes = new List<Shape>();

        foreach (object shapeDefinition in definitions)
        {
            shapes.Add((Shape)shapeDefinition);
        }

        return shapes;
    }
}