using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsingInventory : MonoBehaviour
{
    private Item[] _items;


    private void Awake()
    {
        _items = new Item[6];
    }
}
