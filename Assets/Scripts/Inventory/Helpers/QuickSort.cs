using System;
using System.Collections.Generic;

public static class QuickSort
{
    // Quick Sort
    public static void Sort(List<InventoryItem> items, InventoryManager.SortDirection sortDirection)
    {
        HandleSort(items, 0, items.Count - 1, sortDirection);
    }

    // HELPER: QUICK SORT
    private static void HandleSort(List<InventoryItem> items, int lowEnd, int highEnd, InventoryManager.SortDirection sortDirection)
    {
        if (lowEnd >= highEnd)
        {
            return;
        }
        int partitioningIndex = MakePartition(items, lowEnd, highEnd, sortDirection);

        HandleSort(items, lowEnd, partitioningIndex - 1, sortDirection);
        HandleSort(items, partitioningIndex + 1, highEnd, sortDirection);

    }
    // HELPER: QUICK SORT
    private static int MakePartition(List<InventoryItem> items, int lowEnd, int highEnd, InventoryManager.SortDirection sortDirection)
    {
        float pivotItemHue = items[highEnd].GetShape().Hue;

        int i = (lowEnd - 1);

        for (int j = lowEnd; j <= highEnd - 1; j++)
        {
            bool comparator = sortDirection == InventoryManager.SortDirection.Asc
                ? items[j].GetShape().Hue < pivotItemHue
                : items[j].GetShape().Hue > pivotItemHue;
            if (comparator)
            {
                i++;
                Swap(items, i, j);
            }
        }
        Swap(items, i + 1, highEnd);
        return i + 1;
    }

    // HELPER: QUICK SORT
    private static void Swap(List<InventoryItem> items, int a, int b)
    {
        InventoryItem item = items[a];
        items[a] = items[b];
        items[b] = item;
    }
}

