using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum ItemCode
{
    Minimun,
    Item1,
    Item2,
    Item3,
    Item4,
    Item5,
    Item6,
    Item7,
    Item8,
    Item9,
    Item10,
    Item11,
    Item12,
    Item13,
    Item14,
    Item15,
    Maximun
}

public class ItemSlot : MonoBehaviour
{
    private const int INDEX = 1;
    
    private static SetItems _setItems;

    public int ActivePercent;

    private void Awake()
    {
        _setItems = FindObjectOfType<SetItems>();
    }


    public void AddNewItem(ItemCode itemCode)
    {
        Debug.Assert(itemCode > ItemCode.Minimun && itemCode < ItemCode.Maximun);

        GameObject obj = Instantiate(_setItems.Items[(int)itemCode - INDEX]);
        obj.transform.SetParent(this.transform);
        obj.transform.position = this.transform.position;

        obj.GetComponent<Item>().Code = itemCode; 
    }

    public void ActiveItem(BattlePlayer player, BattlePlayer opponent)
    {
        this.GetComponentInChildren<Item>().Active(player, opponent);
    }

    /// <summary>
    /// todo: 임시, 아이템 사용 했다는것 표시
    /// </summary>
    public void ChangeColor(Color color)
    {
        this.GetComponentInChildren<Item>().ChangeColor(color);
    }

    public void DeleteItem()
    {
        if(this.transform.childCount == 1)
            Destroy(this.transform.GetChild(0).gameObject);
    }
}
