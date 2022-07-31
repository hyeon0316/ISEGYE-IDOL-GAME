using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsingInventory : MonoBehaviour
{
    //todo: 슬롯 별로 확률 지정하기
    private Item[] _items;
    


    private void Awake()
    {
        _items = new Item[6];
    }
}
