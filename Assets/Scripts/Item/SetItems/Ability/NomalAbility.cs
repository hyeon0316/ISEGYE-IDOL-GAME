using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalAbility : Item
{

    public override void Active(BattlePlayer battlePlayer)
    {
        Debug.Log(this.GetType().Name);
    }
}
