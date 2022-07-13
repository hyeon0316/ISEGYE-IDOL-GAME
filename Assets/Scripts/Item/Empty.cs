using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Empty : Item
{
    public override void Awake()
    {
        base.Awake();
        SetData(0);
    }

    public override void SetData(uint itemCode)
    {
        Code = itemCode;
        CurItemType = ItemType.Empty;
    }

    public override void Active()
    {
        Debug.Log("아이템이 비어 있습니다.");
    }
}
