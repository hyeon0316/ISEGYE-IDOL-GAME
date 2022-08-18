using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 방어력 5를 얻음
/// </summary>
public class NomalDefense : Item
{


    public override void Active(BattlePlayer battlePlayer)
    {
        Debug.Log(this.GetType().Name);
    }
}
