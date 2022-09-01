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

    public int ActivePercent;

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

        obj.GetComponent<Item>().Code = itemCode; //아이템 코드 지정 후 전투씬에서 가져오기
    }

    public void ActiveItem(BattlePlayer player, BattlePlayer opponet)
    {
        if (this.transform.childCount == 1)
        {
            this.GetComponentInChildren<Item>().Active(player, opponet);
        }
    }

    /// <summary>
    /// todo: 임시, 아이템 사용 했다는것 표시
    /// </summary>
    public void ChangeColor(Color color)
    {
        if (this.transform.childCount == 1)
        {
            this.GetComponentInChildren<Item>().ChangeColor(color);
        }
    }

    public void DeleteItem()
    {
        if (this.transform.childCount == 1)
        {
            Destroy(this.transform.GetChild(0).gameObject);
        }
    }
}
