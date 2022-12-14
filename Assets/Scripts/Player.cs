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
    public ECharacterType Type = ECharacterType.Empty;

    private int _maxHp;
    private int _hp;
    private int _defense;

    public string NickName { get; private set; }

    private Sprite _sprite;

    public TextMeshProUGUI NameText;
    public TextMeshProUGUI HpText;

    public byte[] ActiveIndex = new byte[60];

    public Int16 FirstAttack { get; set; }

    public Sprite Sprite
    {
        get { return _sprite; }
        set { _sprite = value; }
    }

    [SerializeField] private int _id;

    public int ID => _id;
    public int Hp => _hp;
    public int MaxHp => _maxHp;
    public int Defense
    {
        get { return _defense; }
        set { _defense = value; }
    }

    public UsingInventory UsingInventory;
    public UnUsingInventory UnUsingInventory;


    public void Init(Sprite image, int maxHp, int defense)
    {
        _sprite = image;
        _maxHp = maxHp;
        _hp = _maxHp;
        HpText.text = $"체력: {_hp}";
        _defense = defense;
        
        UnUsingInventory.InitItem();
        UsingInventory.InitItem();
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
    /// 게임 첫 시작시 기본적으로 주워지는 아이템 생성
    /// </summary>
    public void AddDefaultItem()
    {
        UnUsingInventory.AddItem(Global.DefaultItem1);
        UnUsingInventory.AddItem(Global.DefaultItem2);
    }

    /// <summary>
    /// UnUsing의 아이템들을 Using에 자동 셋팅
    /// </summary>
    public void AutoSetItem()
    {
        if (UsingInventory.IsEmpty())
        {
            int index = 0;
            
            for (int i = 0; i < UnUsingInventory.ItemSlots.Length; i++)
            {
                if (index == 6)//아이템이 다 채워졌을 때
                    break;
                
                if (UnUsingInventory.ItemSlots[i].transform.childCount == 1)
                {
                    Item item = UnUsingInventory.ItemSlots[i].GetComponentInChildren<Item>();
                    if (!UsingInventory.CheckDuplication(item))
                    {
                        item.transform.SetParent(UsingInventory.ItemSlots[index].transform);
                        item.transform.position = UsingInventory.ItemSlots[index++].transform.position;
                    }
                }
            }
            
            UnUsingInventory.CheckFullSlot();
        }
    }
    
    /// <summary>
    /// 데미지 또는 회복이후 체력 갱신
    /// </summary>
    public void UpdateHp(int amount)
    {
        _hp += amount;

        if (_hp < 0)
        {
            _hp = 0;
            NetworkManager.Instance.DisconnectServer();
            WindowManager.Instance.SetWindow((int)EWindowType.Lobby);
            //todo: 전부 초기화
        }
        HpText.text = $"체력: {_hp}";
        WindowManager.Instance.Windows[(int)EWindowType.InGame].GetComponent<InGame>().PlayersMap.UpdatePlayersHp(ID);
        
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


    /// <summary>
    /// 싱글용
    /// </summary>
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
        
        UnUsingInventory.CheckFullSlot();
    }

    /// <summary>
    /// 플레이어가 가진 아이템을 랜덤한 순서로 뽑아내는 아이템
    /// </summary>
    public byte[] GetRandItemOrder()
    {
        byte[] result = new byte[Global.ItemQueueLength];
        int index = 0;
        int sum = 0;
        List<byte> slots = new List<byte>();

        for (int i = 0; i < UsingInventory.ItemSlots.Length; ++i)
        {
            if (UsingInventory.ItemSlots[i].transform.childCount == 1)
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
                    result[index++] = 1;
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
                    result[index++] = 255; //나머지 빈 공간
                    result[index++] = 0;
                }
            }
        }
        return result;
    }
}