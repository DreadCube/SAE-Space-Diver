using System;
public static class InsertionSort
{
    // Insertion Sort
    public static void Sort(InventoryItem[] items, InventoryManager.SortDirection sortDirection)
    {
        int arrayLength = items.Length;

        for (int i = 1; i < arrayLength; i++)
        {
            InventoryItem item = items[i];

            int j = i - 1;

            bool comparator = sortDirection == InventoryManager.SortDirection.Asc
                ? items[j].GetAmount() > item.GetAmount()
                : items[j].GetAmount() < item.GetAmount();

            while (j >= 0 && comparator)
            {
                items[j + 1] = items[j];
                j = j - 1;
            }
            items[j + 1] = item;
        }
    }
}

