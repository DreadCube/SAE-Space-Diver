using System.Collections.Generic;

public static class SortShapeType
{
    /**
     * Sorts our Inventory Items based on the Shape Type.
     * 
     * We use a Merge Sort here.
     * 
     * A merge Sort has basically two steps:
     * 
     * First Step: Divide the List of items in two parts (left, right) recursively until
     * theres only one item in the recursive list.
     * 
     * Second Step:
     * We compare the left divided list with the right divided list, sort them
     * and put them together in one list. We repeat this step until we have our
     * full sorted list with all items.
     * 
     * Used reference:
     * https://www.geeksforgeeks.org/merge-sort/
     */
    public static void Sort(List<InventoryItem> items, InventoryManager.SortDirection sortDirection)
    {
        HandleSort(items, 0, items.Count - 1, sortDirection);
    }

    /**
     * HandleSort will be called recursively and divides our full list always
     * in a left and right list.
     * 
     * left and right represents the start index of the sub list.
     * 
     * If we reached the level where our left and right list contains only one element
     * we can start with the Merge.
     */
    private static void HandleSort(List<InventoryItem> items, int left, int right, InventoryManager.SortDirection sortDirection)
    {
        if (left >= right)
        {
            // No more item division needed if theres only one item left.
            return;
        }

        int middlePoint = left + (right - left) / 2;
        HandleSort(items, left, middlePoint, sortDirection);
        HandleSort(items, middlePoint + 1, right, sortDirection);

        Merge(items, left, middlePoint, right, sortDirection);
    }

    /**
     * The Merge part merges both list (left and right) sorted together
     */
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

            /**
             * For SortDirection.Asc:
             * if the shape Type item from the left group is smaller or equal than the
             * shape Type item from the right group we apply the item from the left
             * elsewhere we apply the item from the right
             * 
             * For SortDirection.Desc:
             * If the shape Type item from the left group is bigger or equal than the
             * shape Type item from the right group we apply the item from the left
             * elsewhere we apply the item from the right
             */
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