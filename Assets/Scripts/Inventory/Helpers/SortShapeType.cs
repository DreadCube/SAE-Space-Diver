using System.Collections.Generic;

public static class SortShapeType
{
    /**
     * Sorts our Inventory Items based on the Shape Type.
     * 
     * We use a Merge Sort here.
     * 
     * Used reference:
     * https://www.geeksforgeeks.org/merge-sort/
     */
    public static void Sort(List<InventoryItem> items, InventoryManager.SortDirection sortDirection)
    {
        HandleSort(items, 0, items.Count - 1, sortDirection);
    }

    // Helper: MERGE SORT
    private static void HandleSort(List<InventoryItem> items, int left, int right, InventoryManager.SortDirection sortDirection)
    {
        if (left >= right)
        {
            return;
        }

        int middlePoint = left + (right - left) / 2;
        HandleSort(items, left, middlePoint, sortDirection);
        HandleSort(items, middlePoint + 1, right, sortDirection);

        Merge(items, left, middlePoint, right, sortDirection);
    }

    // Helper: MERGE SORT
    private static void Merge(List<InventoryItem> items, int left, int middlePoint, int right, InventoryManager.SortDirection sortDirection)
    {
        int sizeA = middlePoint - left + 1;
        int sizeB = right - middlePoint;

        List<InventoryItem> tempLeft = new List<InventoryItem>();
        List<InventoryItem> tempRight = new List<InventoryItem>();

        for (int i = 0; i < sizeA; ++i)
        {
            tempLeft.Add(items[left + i]);
        }

        for (int j = 0; j < sizeB; ++j)
        {
            tempRight.Add(items[middlePoint + 1 + j]);
        }

        int index = left;


        int firstIndex = 0;
        int secondIndex = 0;

        while (firstIndex < sizeA && secondIndex < sizeB)
        {
            bool comparator = sortDirection == InventoryManager.SortDirection.Asc
                ? tempLeft[firstIndex].GetShape().Type <= tempRight[secondIndex].GetShape().Type
                : tempLeft[firstIndex].GetShape().Type >= tempRight[secondIndex].GetShape().Type;
            if (comparator)
            {
                items[index] = tempLeft[firstIndex];
                firstIndex++;
            }
            else
            {
                items[index] = tempRight[secondIndex];
                secondIndex++;
            }
            index++;
        }

        while (firstIndex < sizeA)
        {
            items[index] = tempLeft[firstIndex];
            firstIndex++;
            index++;
        }

        while (secondIndex < sizeB)
        {
            items[index] = tempRight[secondIndex];
            secondIndex++;
            index++;
        }
    }
}