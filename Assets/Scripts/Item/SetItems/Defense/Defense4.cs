using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defense4 : Item
{

    public override void Active(BattlePlayer battlePlayer)
    {
        Debug.Log(this.GetType().Name);
        battlePlayer.UpdateAvatarHp(-10);//임시
        
        ChangeColor(Color.black);
    }
    

    public override void ChangeColor(Color color)
    {
        Image.color = color;
    }
}
