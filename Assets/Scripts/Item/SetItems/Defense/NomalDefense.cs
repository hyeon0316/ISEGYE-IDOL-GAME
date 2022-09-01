using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 방어력 5를 얻음
/// </summary>
public class NomalDefense : Item
{
    public override void Active(BattlePlayer player, BattlePlayer opponent)
    {
        Debug.Log(this.GetType().Name);
        opponent.UpdateAvatarHp(-10);//임시
        
        ChangeColor(Color.black);
    }
    

    public override void ChangeColor(Color color)
    {
        Image.color = color;
    }
}
