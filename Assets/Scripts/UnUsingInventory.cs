using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UnUsingInventory : MonoBehaviour
{
    public ItemSlot[] ItemSlots;
    

    /// <summary>
    /// 100% 확률 버튼
    /// </summary>
    public void AddRandomItem() //로컬  
    {
        int rand = Random.Range(1, 16); //SetItems에 지정되어있는 0~14번째 인덱스중 하나 선택
        
        AddItem(rand);
        NetworkManager.Instance.SendAddNewItemPacket(PlayerManager.Instance.Players[0].ID,(byte)rand);

        CheckFullSlot();
    }


    public void AddItem(int itemCode)
    {
        for (int i = 0; i < ItemSlots.Length; i++)
        {
            if (ItemSlots[i].transform.childCount == 1)
                continue;

            ItemSlots[i].AddNewItem(itemCode);
            break;
        }
    }

    public void CheckFullSlot()
    {
        int fullCheck = 0;
        for (int i = 0; i < ItemSlots.Length; i++)
        {
            if (ItemSlots[i].transform.childCount == 1)
                ++fullCheck;
        }

        if (fullCheck == Global.SlotMaxCount)
            GameObject.Find("RouletteBtn").GetComponent<Button>().interactable = false;
        else
            GameObject.Find("RouletteBtn").GetComponent<Button>().interactable = true;
    }
}
