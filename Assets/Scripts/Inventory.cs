using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Inventory : MonoBehaviour
{
    public ItemSlot[] ItemSlots;
    
    public void InitItem()
    {
        foreach (var item in ItemSlots)
        {
            item.DeleteItem();
        }
    }
}
