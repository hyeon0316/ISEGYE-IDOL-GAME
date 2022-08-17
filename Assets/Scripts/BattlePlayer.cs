using System;
using System.Collections;
using System.Collections.Generic;
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
    

    public void SetBattlePlayer(Player player, byte[] itemOrder)
    {
        Player = player;
        _itemOrder = itemOrder;
        
        Player.SetItem(ItemSlots);
        AvatarHp = 100;
        AvatarHpText.text = $"아바타 체력: {AvatarHp}";
        AvatarImage.sprite = Player.Sprite;
        PlayerNickName.text = $"{Player.NickName}";

        foreach (var v in _itemOrder)
        {
            Debug.Log($"{Player.NickName}{v}");
        }
    }

    public void UpdateAvatarHp(int amount)
    {
        AvatarHp += amount;
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
            do
            {
                itemSlot = _itemOrder[_index++];
                active = (bool)_itemOrder[_index++];
                if (_index == _itemOrder.Length)
                    _index = 0;
            } while (itemSlot == 255);
            
            if (active)
            {
                ItemSlots[itemSlot].ActiveItem();
            }
        }

        _isMyturn = !_isMyturn;
    }
}
