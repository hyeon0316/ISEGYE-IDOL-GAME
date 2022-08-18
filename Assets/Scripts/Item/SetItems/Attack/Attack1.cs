using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack1 : Item
{

    public override void Active(BattlePlayer battlePlayer)
    {
        Debug.Log(this.GetType().Name);
    }
}
