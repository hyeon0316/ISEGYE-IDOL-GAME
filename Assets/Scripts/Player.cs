using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    public CharacterType Type;

    private int _hp;
    private int _defense;

    public string NickName { get; private set; }

    private Sprite _sprite;

    public TextMeshProUGUI NameText;
    public TextMeshProUGUI HpText;

    public Sprite Sprite
    {
        get { return _sprite; }
        set { _sprite = value; }
    }

    [SerializeField] private int _id;

    public int ID => _id;
    public int Hp => _hp;

    public UsingInventory UsingInventory;
    public UnUsingInventory UnUsingInventory;


    public void Init(Sprite image, int hp, int defense)
    {
        _sprite = image;
        _hp = hp;
        HpText.text = $"체력: {_hp}";
        _defense = defense;
    }

    public void SetName(string name)
    {
        NickName = name;
        NameText.text = NickName;
    }

    public void SetID(int id)
    {
        _id = id;
    }
    
    /// <summary>
    /// 데미지 또는 회복이후 체력 갱신
    /// </summary>
    public void UpdateHp(int amount)
    {
        _hp += amount;
        HpText.text = $"체력: {_hp}";
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
        ItemSlot itemSlot1 = slotIndex1 < 6
            ? UsingInventory.ItemSlots[slotIndex1]
            : UnUsingInventory.ItemSlots[slotIndex1 - 6];
        ItemSlot itemSlot2 = slotIndex2 < 6
            ? UsingInventory.ItemSlots[slotIndex2]
            : UnUsingInventory.ItemSlots[slotIndex2 - 6];

        Item dragItem = GameObject.Find("DragItem").GetComponentInChildren<Item>();

        if (itemSlot2.transform.childCount == 1)
        {
            Item tempItem = itemSlot2.GetComponentInChildren<Item>();
            tempItem.RePosItem(dragItem.OriginParent, dragItem.OriginPos);
        }

        dragItem.RePosItem(itemSlot2.transform, itemSlot2.transform.position);
    }

    public void SwapItemNetwork(int slotIndex1, int slotIndex2)
    {
        ItemSlot itemSlot1 = slotIndex1 < 6
            ? UsingInventory.ItemSlots[slotIndex1]
            : UnUsingInventory.ItemSlots[slotIndex1 - 6];
        ItemSlot itemSlot2 = slotIndex2 < 6
            ? UsingInventory.ItemSlots[slotIndex2]
            : UnUsingInventory.ItemSlots[slotIndex2 - 6];

        if (itemSlot2.transform.childCount == 1)
        {
            Item tempItem = itemSlot2.GetComponentInChildren<Item>();
            tempItem.RePosItem(itemSlot1.transform, Vector3.zero);
            tempItem.transform.localPosition = Vector3.zero;
        }

        Item tempItem2 = itemSlot1.GetComponentInChildren<Item>();
        tempItem2.RePosItem(itemSlot2.transform, Vector3.zero);
        tempItem2.transform.localPosition = Vector3.zero;
    }

    /// <summary>
    /// 플레이어가 가진 아이템을 랜덤한 순서로 뽑아내는 아이템
    /// </summary>
    public byte[] GetRandItemOrder()
    {
        byte[] result = new byte[30];
        int index = 0;
        int sum = 0;
        List<byte> slots = new List<byte>();

        for (int i = 0; i < UsingInventory.ItemSlots.Length; ++i)
        {
            if (UsingInventory.ItemSlots[i].transform.childCount ==1)
            {
                slots.Add((byte) i);
                sum += UsingInventory.ItemSlots[i].ActivePercent;
            }
        }

        int length = slots.Count;
        for (int i = 0; i < length * 4; i++)
        {
            slots.Add(slots[i % length]);
        }

        int loopSum = sum;
        int loopLength = length;
        while (slots.Count > 0)
        {
            int rand = Random.Range(0, loopSum);
            for (int i = 0; i < loopLength; i++)
            {
                int percent = UsingInventory.ItemSlots[slots[i]].ActivePercent;
                rand -= percent;
                if (rand < 0)
                {
                    result[index++] = slots[i];
                    loopSum -= percent;
                    slots.Remove(slots[i]);
                    loopLength--;
                    break;
                }
            }
            if (loopLength == 0)
            {
                loopSum = sum;
                loopLength = length;
                for (int i = 0; i < UsingInventory.ItemSlots.Length - length; i++)
                {
                    result[index++] = 255;//나머지 빈 공간
                }
            }
        }

        return result;
    }
}