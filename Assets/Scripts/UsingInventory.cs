using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsingInventory : MonoBehaviour
{
    //todo: 슬롯 별로 확률 지정하기
    public ItemSlot[] ItemSlots;


    private void Awake()
    {
        ItemSlots = this.GetComponentsInChildren<ItemSlot>();
    }

}
