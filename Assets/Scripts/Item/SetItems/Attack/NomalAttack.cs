using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// 적에게 데미지 10을 줌
/// </summary>
public class NomalAttack : Item
{

   

   public override void Active(BattlePlayer battlePlayer)
   {
      Debug.Log(this.GetType().Name);
   }
}
