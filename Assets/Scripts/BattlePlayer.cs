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
    
    public void SetBattlePlayer(Player player, byte[] itemOrder, BattlePlayer oppoent)
    {
        Player = player;
        _itemOrder = itemOrder;
        Opponent = oppoent;
        
        Player.SetItem(ItemSlots);
        _isMyturn = false;
        AvatarHp = 100;
        AvatarHpText.text = $"아바타 체력: {AvatarHp}";
        DefenseText.text = $"방어력: {Player.Defense}";
        AvatarImage.sprite = Player.Sprite;
        PlayerNickName.text = $"{Player.NickName}";
        _index = 0;
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
        }

        AvatarHpText.text = $"아바타 체력: {AvatarHp}";
    }

    public void UpdateDefense(int amount)
    {
        _remainNum = 0;
        Player.Defense += amount;
        if (Player.Defense < 0) //todo: 초과 기준점도 생각하기
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

    public void ActiveItem()
    {
        if (_isMyturn)
        {
            int itemSlot;
            bool active = false;

            itemSlot = _itemOrder[_index++];
            active = Convert.ToBoolean(_itemOrder[_index++]);
            if (_index == _itemOrder.Length)
                _index = 0;

            if (active)
            {
                if(itemSlot == 255)
                    Debug.Log("비어 있음");
                else
                {
                    ItemSlots[itemSlot].ActiveItem(this, Opponent);
                }
            }
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
}
