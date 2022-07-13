using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnUsingInventory : MonoBehaviour
{
    //처음에 여기에 아이템이 먼저 추가됨
    public ItemSlot[] ItemSlots; 
    
    private void Awake()
    {
        ItemSlots = this.GetComponentsInChildren<ItemSlot>();
        AddNewItem(1);
    }

    private void AddNewItem(uint itemCode)
    {
        ItemSlots[0].AddNewItem(itemCode);
    }
}
