using System.Collections.Generic;

public static class SortAmount
{
    /**
     * Sorts our Inventory Items based on Amount.
     * 
     * We use a Insertion Sort here.
     * 
     * Insertion sort is not the fastest sorting algorhytm for bigger data assets
     * but its easy to understand and easy to implement.
     * Because we have a small inventory item count i think its fine for our use case.
     * 
     * Used reference:
     * https://www.geeksforgeeks.org/insertion-sort/
     */
    public static void Sort(List<InventoryItem> items, InventoryManager.SortDirection sortDirection)
    {
        int itemsLength = items.Count;

        // Nothing to do if theres only one Element
        if (itemsLength <= 1)
        {
            return;
        }

        /**
         * The for Loop represents the current cursor Element that
         * will be compared later on with the predecessors
         */
        for (int i = 1; i < itemsLength; ++i)
        {
            InventoryItem item = items[i];
            int itemAmount = item.GetAmount();

            int j = i - 1;

            /**
             * For SortDirection.Asc: 
             * As long as our cursor element is smaller
             * than the predecessor we swap it with the predecessor.
             * 
             * For Sortdirection.Desc:
             * As long as our cursor element is bigger
             * than the predecessor we swap it with the predecessor.
             */
            while (j >= 0)
            {
                bool comparator = sortDirection == InventoryManager.SortDirection.Asc
                    ? items[j].GetAmount() > itemAmount
                    : items[j].GetAmount() < itemAmount;

                /**
                 * We can break out of the while loop
                 * if our cursor element is now at the correct position
                 */
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

