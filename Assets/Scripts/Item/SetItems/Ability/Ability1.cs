using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 귀족의 상현딸
/// </summary>
public class Ability1 : Item
{
    private int _damage = -10;
    
    public override void Active(BattlePlayer player, BattlePlayer opponent)
    {
        Debug.Log(this.GetType().Name);
        opponent.ActiveCC(_damage);
        
        ChangeColor(Color.black);
    }
    
}
