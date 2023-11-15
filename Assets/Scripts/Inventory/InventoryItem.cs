using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem
{
    private Shape item;
    private int amount;

    public InventoryItem(Shape item, int amount = 0)
    {
        this.item = item;
        this.amount = amount;
    }

    public int GetAmount()
    {
        return amount;
    }

    public Shape GetShape()
    {
        return item;
    }
}
