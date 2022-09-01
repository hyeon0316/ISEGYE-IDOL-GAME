using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsingInventory : MonoBehaviour
{
    //todo: 슬롯 별로 확률 지정하기
    public ItemSlot[] ItemSlots;



    /// <summary>
    /// 6개의 슬롯 모두 아이템이 없는지 확인
    /// </summary>
    public bool IsEmpty()
    {
        for (int i = 0; i < ItemSlots.Length; i++)
        {
            if (ItemSlots[i].transform.childCount == 1)
                return false;
        }
        return true;
    }

    /// <summary>
    /// 아이템이 중복으로 셋팅되어 있는지 체크
    /// </summary>
    public bool CheckDuplication(Item checkItem)
    {
        for (int i = 0; i < ItemSlots.Length; i++)
        {
            if (ItemSlots[i].transform.childCount == 1)
            {
                if (checkItem.Code == ItemSlots[i].GetComponentInChildren<Item>().Code)
                    return true;
            }

        }
        return false;
    }

    public void InitItem()
    {
        foreach (var item in ItemSlots)
        {
            item.DeleteItem();
        }
    }
}
