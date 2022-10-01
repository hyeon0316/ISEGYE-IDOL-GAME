using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 다이아 검
/// </summary>
public class Attack4 : Item
{
    private int _damage = -10;

    public override void Active(BattlePlayer player, BattlePlayer opponent)
    {
        Debug.Log(this.GetType().Name);
        opponent.TakeDiaSwordDamage(_damage);
        
        ChangeColor(Color.black);
        player.NextActiveItem();
    }
   

   
}
