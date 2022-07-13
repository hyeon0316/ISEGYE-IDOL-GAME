using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defense : Item
{
    public override void Awake()
    {
        base.Awake();
    }

    public override void SetData(uint itemCode)
    {
        Code = itemCode;
        CurItemType = ItemType.Defense;
        SetItem = new SetItem(itemCode);
    }

    public override void Active()
    {
        if (Code == 6)
        {}
        else if(Code == 7)
        {}
        else if(Code == 8)
        {}
        else if(Code == 9)
        {}
        else if(Code == 10)
        {}
    }
}
