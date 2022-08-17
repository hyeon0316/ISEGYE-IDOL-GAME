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
    
    private const int DEFAULT_ITEMCODE1 = 1;
    private const int DEFAULT_ITEMCODE2 = 6;

    private const int SLOT_MAX = 10;

    
    private void Awake()
    {
        ItemSlots = this.GetComponentsInChildren<ItemSlot>();
    }

    private void Start()
    {
        //기본 아이템 미리 생성
        AddItem(DEFAULT_ITEMCODE1);
        AddItem(DEFAULT_ITEMCODE2);
    }

    private void Update()
    {
        CheckFull();
    }


    /// <summary>
    /// 100% 확률 버튼
    /// </summary>
    public void AddRandomItem() //로컬  
    {
        int rand = Random.Range(1, 16); //SetItems에 지정되어있는 0~14번째 인덱스중 하나 선택
        
        AddItem(rand);
        //todo: 서버에 데이터 전송
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

    private void CheckFull()
    {
        //todo: 업데이트로 계속 돌리지 않고 아이템 옮길 때만 검사하기
        if (this.transform.parent.name == "Ready")
        {
            int fullCheck = 0;
            for (int i = 0; i < ItemSlots.Length; i++)
            {
                if (ItemSlots[i].transform.childCount == 1)
                    ++fullCheck;
            }

            if (fullCheck == SLOT_MAX)
                GameObject.Find("RouletteBtn").GetComponent<Button>().interactable = false;
            else
                GameObject.Find("RouletteBtn").GetComponent<Button>().interactable = true;
        }
    }
}
