using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    private const int MIN_CODE_RANGE = 1;
    private const int MAX_CODE_RANGE = 15;
    private const int INDEX = 1;
    
    private static SetItems _setItems;

    private void Awake()
    {
        _setItems = FindObjectOfType<SetItems>();
    }


    public void AddNewItem(int itemCode)
    {
        Debug.Assert(itemCode >= MIN_CODE_RANGE && itemCode <= MAX_CODE_RANGE);

        GameObject obj = Instantiate(_setItems.Items[itemCode - INDEX]);
        obj.transform.SetParent(this.transform);
        obj.transform.position = this.transform.position;
    }
}
