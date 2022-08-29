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
    

    public void SetBattlePlayer(Player player, byte[] itemOrder)
    {
        Player = player;
        _itemOrder = itemOrder;
        
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
                    ItemSlots[itemSlot].ActiveItem(this);
                    ItemSlots[itemSlot].DeleteItem();//임시
                }
            }
        }

        _isMyturn = !_isMyturn;
    }
}
