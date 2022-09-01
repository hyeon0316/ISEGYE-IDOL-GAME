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
    
    public Player Player { get; private set; }
    private byte[] _itemOrder;
    private int _index = 0;
    private bool _isMyturn = false;

    public BattlePlayer Opponent { get; set; }

    public void SetBattlePlayer(Player player, byte[] itemOrder, BattlePlayer oppoent)
    {
        Player = player;
        _itemOrder = itemOrder;
        Opponent = oppoent;
        
        Player.SetItem(ItemSlots);
        _isMyturn = false;
        AvatarHp = 100;
        AvatarHpText.text = $"아바타 체력: {AvatarHp}";
        AvatarImage.sprite = Player.Sprite;
        PlayerNickName.text = $"{Player.NickName}";
        _index = 0;
    }

    public void UpdateAvatarHp(int amount)
    {
        AvatarHp += amount;
        if (AvatarHp < 0)
            AvatarHp = 0;
        AvatarHpText.text = $"아바타 체력: {AvatarHp}";
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
        foreach (var slot in ItemSlots)
            slot.ChangeColor(Color.white);
    }
    
    public void DeleteItem()
    {
        for (int i = 0; i < ItemSlots.Length; i++)
        {
            if(ItemSlots[i].transform.childCount == 1)
                ItemSlots[i].DeleteItem();
        }
    }
}
