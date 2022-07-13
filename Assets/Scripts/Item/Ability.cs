using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : Item
{
    public override void Awake()
    {
        base.Awake();
    }

    public override void SetData(uint itemCode)
    {
        Code = itemCode;
        CurItemType = ItemType.Ability;
        SetItem = new SetItem(itemCode);
    }

    public override void Active()
    {
        if (Code == 11)
        {}
        else if(Code == 12)
        {}
        else if(Code == 13)
        {}
        else if(Code == 14)
        {}
        else if(Code == 15)
        {}
    }
}
