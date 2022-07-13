using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    private Item _item;
    public Item Item => _item;

    private const int ATTACK_RANGE = 5;
    private const int DEFENSE_RANGE = 10;
    private const int ABILITY_RANGE = 15;


    private void Awake()
    {
        _item = this.GetComponentInChildren<Item>();
    }

    public void AddNewItem(uint itemCode)
    {
        if (_item.CurItemType == ItemType.Empty)
        {
            Destroy(_item.GetComponent<Item>());
            
            if (itemCode <= ATTACK_RANGE)
                _item.AddComponent<Attack>();
            else if (itemCode > ATTACK_RANGE && itemCode <= DEFENSE_RANGE)
                _item.AddComponent<Defense>();
            else if (itemCode > DEFENSE_RANGE && itemCode <= ABILITY_RANGE)
                _item.AddComponent<Ability>();
            
            _item.SetData(itemCode);
        }
        else
            return;
    }
}
