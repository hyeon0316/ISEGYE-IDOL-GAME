using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    private int _hp;
    private int _defense;

    public string NickName;

    private Sprite _sprite;
    public Sprite Sprite => _sprite;
    
    [SerializeField]
    private int _id;

    public int ID => _id;
    public int Hp => _hp;

    public UsingInventory UsingInventory;
    public UnUsingInventory UnUsingInventory;


    public void SetStat(Sprite image, int hp, int defense, string name)
    {
        _sprite = image;
        _hp = hp;
        _defense = defense;
        NickName = name;
    }

    public void SetID(int id)
    {
        _id = id;
    }

    /// <summary>
    /// 준비단계에서 셋팅한 아이템을 전투단계에 사용
    /// </summary>
    public void SetItem(ItemSlot[] usingSlots)
    {
        for (int i = 0; i < UsingInventory.ItemSlots.Length; i++)
        {
            if (UsingInventory.ItemSlots[i].transform.childCount == 1)
            {
                usingSlots[i].AddNewItem(UsingInventory.ItemSlots[i].GetComponentInChildren<Item>().Code);
            }
        }
    }


    public void SwapItem(int slotIndex1, int slotIndex2)
    {
        ItemSlot itemSlot1 = slotIndex1 < 6 ? UsingInventory.ItemSlots[slotIndex1] : UnUsingInventory.ItemSlots[slotIndex1 - 6];
        ItemSlot itemSlot2 = slotIndex2 < 6 ? UsingInventory.ItemSlots[slotIndex2] : UnUsingInventory.ItemSlots[slotIndex2 - 6];

        Item dragItem = GameObject.Find("DragItem").GetComponentInChildren<Item>();

        if (itemSlot2.transform.childCount == 1)
        {
            Item tempItem = itemSlot2.GetComponentInChildren<Item>();       
            tempItem.RePosItem(dragItem.OriginParent, dragItem.OriginPos);
        }

        dragItem.RePosItem(itemSlot2.transform, itemSlot2.transform.position);
    }
}
