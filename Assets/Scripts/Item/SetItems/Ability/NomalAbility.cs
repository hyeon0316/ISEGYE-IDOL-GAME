using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 낡은 채찍
/// </summary>
public class NomalAbility : Item
{

    public override void Active(BattlePlayer player, BattlePlayer opponent)
    {
        Debug.Log(this.GetType().Name);
        opponent.UpdateAvatarHp(-10);//임시
        
        ChangeColor(Color.black);
        player.NextActiveItem();
    }
    

   
}
