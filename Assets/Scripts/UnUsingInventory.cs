using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;


public class UnUsingInventory : Inventory
{
    [Header("아이템 선택")]
    public EItemCode CurItemCode;
    
    /// <summary>
    /// 100% 확률 버튼
    /// </summary>
    public void AddRandomItem() //로컬  
    {
        EItemCode rand = (EItemCode)Random.Range((int)EItemCode.Minimun + 1, (int)EItemCode.Maximun); //SetItems에 지정되어있는 0~14번째 인덱스중 하나 선택
        
        AddItem(rand);
        NetworkManager.Instance.SendAddNewItemPacket(PlayerManager.Instance.Players[0].ID,(byte)rand);

        CheckFullSlot();
    }


    public void AddItem(EItemCode itemCode)
    {
        for (int i = 0; i < ItemSlots.Length; i++)
        {
            if (ItemSlots[i].transform.childCount == 1)
                continue;

            ItemSlots[i].AddNewItem(itemCode);
            break;
        }
    }

    /// <summary>
    /// Unusing의 슬롯에 모두 아이템이 존재 하는지 체크하여 뽑기 버튼 활성화 여부 판단
    /// </summary>
    public void CheckFullSlot()
    {
        int fullCheck = 0;
        foreach (var slot in ItemSlots)
        {
            if (slot.transform.childCount == 1)
                ++fullCheck;
        }
        
        WindowManager.Instance.Windows[(int)EWindowType.InGame].GetComponentInChildren<Ready>().TryInteractRoulette(fullCheck);
    }
}
