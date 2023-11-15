using System;
public static class MergeSort
{
    // Merge Sort
    public static void Sort(InventoryItem[] items, InventoryManager.SortDirection sortDirection)
    {
        HandleSort(items, 0, items.Length - 1, sortDirection);
    }

    // Helper: MERGE SORT
    private static void HandleSort(InventoryItem[] items, int left, int right, InventoryManager.SortDirection sortDirection)
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
    private static void Merge(InventoryItem[] items, int left, int middlePoint, int right, InventoryManager.SortDirection sortDirection)
    {
        int sizeA = middlePoint - left + 1;
        int sizeB = right - middlePoint;


        InventoryItem[] tempLeft = new InventoryItem[sizeA];
        InventoryItem[] tempRight = new InventoryItem[sizeB];

        for (int i = 0; i < sizeA; ++i)
        {
            tempLeft[i] = items[left + i];
        }

        for (int j = 0; j < sizeB; ++j)
        {
            tempRight[j] = items[middlePoint + 1 + j];
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