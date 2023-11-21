using System.Collections.Generic;

public static class SortHue
{
    /**
     * Sorts our inventory items based on the Hue.
     * 
     * We use a quick sort here. In nature quick sort is unstable. But here we have
     * a quick sort variation that is stable. 
     * So: items with the same hue will stay at the same position.
     * 
     * A stable Quick Sort Algorhytm takes more Space Complexity than the
     * unstable Quick Sort because Items has to be stored in temporary Lists.
     * But i think its tolerable because
     * the amount of items we have to sort is small and we still wanna benefit from
     * the fast Quick Sort ;-)
     * 
     * Used resource:
     * https://www.geeksforgeeks.org/stable-quicksort/
     */
    public static void Sort(List<InventoryItem> items, InventoryManager.SortDirection sortDirection)
    {
        List<InventoryItem> resultItems = QuickSortStable(items, sortDirection);

        items.Clear();
        items.InsertRange(0, resultItems);
    }

    /**
     * Quick Sort is a recursive sorting algorhytm and works with a pivot.
     * In our case the pivot is always the middle element.
     * Smaller items than the pivot will moved to the left (list). Bigger items than the
     * pivot will be moved to the right (list) (or vice versa for the other direction)
     * 
     * We repeat this process with the left and right list recursive until we have our sorted complete List
     */
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
                        continue;
                    }
                    greaterItems.Add(item);
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

