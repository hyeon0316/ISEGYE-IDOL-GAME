using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlayer : MonoBehaviour
{
    public Player Player { get; private set; }

    private byte[] _itemOrder;

    private int index = 0;

    private bool myturn = false;

    public void SetBattlePlayer(Player player, byte[] itemOrder)
    {
        Player = player;
        _itemOrder = itemOrder;
    }

    public void SetFirstTurn()
    {
        myturn = true;
    }

    public void ActiveItem()
    {
        if (myturn)
        {
            int itemSlot;
            do
            {
                itemSlot = _itemOrder[index++];
                if (index == _itemOrder.Length)
                    index = 0;
            } while (itemSlot == 255);
            Player.UsingInventory.ItemSlots[itemSlot].ActiveItem();
        }

        myturn = !myturn;
    }
}