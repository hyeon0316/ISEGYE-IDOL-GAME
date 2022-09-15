using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattlePlayer : MonoBehaviour
{
    public int MaxAvaterHp;
    public int AvatarHp;//전투용 Hp
    public ItemSlot[] ItemSlots;
    public Image AvatarImage;
    public TextMeshProUGUI PlayerNickName;
    public TextMeshProUGUI AvatarHpText;
    public TextMeshProUGUI DefenseText;
    
    public Player Player { get; private set; }
    private byte[] _itemOrder;
    private int _index = 0;
    private bool _isMyturn = false;

    public BattlePlayer Opponent { get; set; }

    private int _remainNum;
    
    public bool IsNextRound { get; set; }
    
    public int UsingCount { get; set; }
    
    public void SetBattlePlayer(Player player, byte[] itemOrder, BattlePlayer oppoent)
    {
        Player = player;
        _itemOrder = itemOrder;
        Opponent = oppoent;
        
        Player.SetItem(ItemSlots);
        _isMyturn = false;
        MaxAvaterHp = 100;
        AvatarHp = MaxAvaterHp;
        AvatarHpText.text = $"아바타 체력: {AvatarHp}";
        DefenseText.text = $"방어력: {Player.Defense}";
        AvatarImage.sprite = Player.Sprite;
        PlayerNickName.text = $"{Player.NickName}";
        _index = 0;
        UsingCount = 0;
    }

    public void UpdateAvatarHp(int amount)
    {
        if (amount < 0) //데미지
        {
            UpdateDefense(amount);
            AvatarHp += _remainNum;
            
            if (AvatarHp < 0) //todo: 초과 기준점도 생각하기
                AvatarHp = 0;
        }
        else //회복
        {
            AvatarHp += amount;
            if (AvatarHp > MaxAvaterHp)
                AvatarHp = MaxAvaterHp;
        }

        AvatarHpText.text = $"아바타 체력: {AvatarHp}";
    }

    public void UpdateDefense(int amount)
    {
        _remainNum = 0;
        Player.Defense += amount;
        if (Player.Defense < 0) 
        {
            _remainNum = Player.Defense;
            Player.Defense = 0;
        }

        DefenseText.text = $"방어력: {Player.Defense}";
    }

    public void SetFirstTurn()
    {
        _isMyturn = true;
    }

    public void SetDisabledTurn()
    {
        _isMyturn = false;
    }

    public void ActiveItem()
    {
        if (_isMyturn)
        {
            int itemSlot;
            bool active = false;

            itemSlot = _itemOrder[_index++];
            active = Convert.ToBoolean(_itemOrder[_index++]); //나중에 확률아이템에 대한 bool값
            if (_index == _itemOrder.Length)
                _index = 0;

            if (active) 
            {
                ItemSlots[itemSlot].ActiveItem(this, Opponent);
                if (_itemOrder[_index] == Global.EmptySlotIndex) // 발동된 아이템의 다음 차례가 빈 슬롯일 경우
                {
                    while (_itemOrder[_index] == Global.EmptySlotIndex)
                    {
                        _index += 2;
                        if (_index >= _itemOrder.Length)
                            _index = 0;
                    }
                }
            }
            else
            {
                Debug.Log("아이템 발동 실패");
                //todo: 아이템 실패 표시
            }
            UsingCount++;
        }

        _isMyturn = !_isMyturn;
    }

    /// <summary>
    /// 다음 6번의 랜덤 발동을 위한 아이템 셋팅
    /// </summary>
    public void UseNextItem()
    {
        foreach (var itemSlot in ItemSlots)
        {
            if(itemSlot.transform.childCount == 1)
                itemSlot.ChangeColor(Color.white);
        }
    }
    
    public void DeleteItem()
    {
        foreach (var itemSlot in ItemSlots)
        {
            if(itemSlot.transform.childCount == 1)
                itemSlot.DeleteItem();
        }
    }

    public int CountItem()
    {
        int count = 0;
        foreach (var itemSlot in ItemSlots)
        {
            if (itemSlot.transform.childCount == 1)
                count++;
        }

        return count;
    }
}
