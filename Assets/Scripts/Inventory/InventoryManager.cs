using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public enum SortDirection
    {
        Asc,
        Desc
    }

    private enum SortType
    {
        Amount,
        Hue,
        ShapeType
    }

    private InventoryItem[] inventoryItems;

    private SortDirection activeSortDirection = SortDirection.Asc;
    private SortType activeSortType = SortType.ShapeType;


    private void Awake()
    {
        object[] definitions = Resources.LoadAll("ShapeDefinitions", typeof(Shape));

        System.Array.Resize(ref inventoryItems, definitions.Length);

        for (int i = 0; i < definitions.Length; i++)
        {
            inventoryItems[i] = new InventoryItem((Shape)definitions[i], Random.Range(0, 10));
        }

        SortList(activeSortType, activeSortDirection);
        RenderList();
    }

    private void RenderList()
    {

        Debug.Log("-------------START---------------");
        Debug.Log($"Sort Direction: {activeSortDirection}");
        Debug.Log($"Sort Type: {activeSortType}");

        foreach (InventoryItem item in inventoryItems)
        {
            Debug.Log($"AMOUNT: {item.GetAmount()} / HUE: {item.GetShape().Hue} / TYPE: {item.GetShape().Type}");
        }
        Debug.Log("-------------END-----------------");
    }

    private void SortList(SortType sortType, SortDirection sortDirection)
    {
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
    }
}