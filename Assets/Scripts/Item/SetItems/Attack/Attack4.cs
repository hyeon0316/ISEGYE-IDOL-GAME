using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack4 : Item
{

    public override void Active(BattlePlayer player, BattlePlayer opponent)
    {
        Debug.Log(this.GetType().Name);
        opponent.UpdateAvatarHp(-10);//임시
        
        ChangeColor(Color.black);
    }
    

   
}
