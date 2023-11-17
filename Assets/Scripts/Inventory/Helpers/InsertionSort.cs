using System.Collections.Generic;

public static class InsertionSort
{
    public static void Sort(List<InventoryItem> items, InventoryManager.SortDirection sortDirection)
    {
        int arrayLength = items.Count;

        for (int i = 1; i < arrayLength; ++i)
        {
            InventoryItem item = items[i];

            int j = i - 1;

            while (j >= 0)
            {
                bool comparator = sortDirection == InventoryManager.SortDirection.Asc
                    ? items[j].GetAmount() > item.GetAmount()
                    : items[j].GetAmount() < item.GetAmount();

                if (!comparator)
                {
                    break;
                }

                items[j + 1] = items[j];
                j = j - 1;
            }
            items[j + 1] = item;
        }
    }
}

