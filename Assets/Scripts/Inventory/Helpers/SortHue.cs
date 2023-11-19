using System.Collections.Generic;

public static class SortHue
{

    /**
     * Sorts our inventory items based on the Hue / Color.
     * 
     * We use a quick sort here. In nature quick sort is unstable. But here we have
     * a stable quick sort implemented.
     * 
     * Used resource:
     * https://www.geeksforgeeks.org/stable-quicksort/
     */
    public static void Sort(List<InventoryItem> items, InventoryManager.SortDirection sortDirection)
    {
        List<InventoryItem> resultItems = QuickSortStable(items, sortDirection);

        items.Clear();
        foreach (InventoryItem item in resultItems)
        {
            items.Add(item);
        }
    }

    private static List<InventoryItem> QuickSortStable(List<InventoryItem> items, InventoryManager.SortDirection sortDirection)
    {
        if (items.Count <= 1)
        {
            return items;
        }

        int middlePointIndex = items.Count / 2;

        InventoryItem pivotElement = items[middlePointIndex];

        float pivotElementHue = pivotElement.GetShape().Hue;

        List<InventoryItem> smallerItems = new List<InventoryItem>();
        List<InventoryItem> greaterItems = new List<InventoryItem>();


        for (int index = 0; index < items.Count; index++)
        {
            InventoryItem item = items[index];
            if (index != middlePointIndex)
            {
                float itemHue = item.GetShape().Hue;
                bool comparator = sortDirection == InventoryManager.SortDirection.Asc
                    ? itemHue < pivotElementHue
                    : itemHue > pivotElementHue;

                if (itemHue == pivotElementHue)
                {
                    if (index < middlePointIndex)
                    {
                        smallerItems.Add(item);
                    }
                    else
                    {
                        greaterItems.Add(item);
                    }
                }
                else if (comparator)
                {
                    smallerItems.Add(item);
                }
                else
                {
                    greaterItems.Add(item);

                }
            }
        }

        List<InventoryItem> resultItems = new List<InventoryItem>();

        List<InventoryItem> resultItemsSmaller = QuickSortStable(smallerItems, sortDirection);
        List<InventoryItem> resultItemsLarger = QuickSortStable(greaterItems, sortDirection);

        foreach (InventoryItem item in resultItemsSmaller)
        {
            resultItems.Add(item);
        }

        resultItems.Add(pivotElement);

        foreach (InventoryItem item in resultItemsLarger)
        {
            resultItems.Add(item);
        }

        return resultItems;
    }
}

