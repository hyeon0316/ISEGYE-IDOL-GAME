using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Attack : Item
{
    public override void Awake()
    {
        base.Awake();
    }

    public override void SetData(uint itemCode)
    {
        Code = itemCode;
        CurItemType = ItemType.Attack;
        SetItem = new SetItem(itemCode);
    }

    public override void Active()
    {
        if (Code == 1)
        {
            
        }
        else if (Code == 2)
        {
            
        }
        else if (Code == 3)
        {
            
        }
        else if(Code == 4)
        {
            
        }
        else if (Code == 5)
        {
            
        }
    }
}
