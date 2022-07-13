using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetItem
{
   /// <summary>
   /// 공격 아이템
   /// </summary>
   public NomalAttack NomalAttack;
   public Attack1 Attack1;
   public Attack2 Attack2;
   public Attack3 Attack3;
   public Attack4 Attack4;
   
   /// <summary>
   /// 수비 아이템
   /// </summary>
   public NomalDefense NomalDefense;
   public Defense1 Defense1;
   public Defense2 Defense2;
   public Defense3 Defense3;
   public Defense4 Defense4;

   /// <summary>
   /// 능력 아이템
   /// </summary>
   public NomalAbility NomalAbility;
   public Ability1 Ability1;
   public Ability2 Ability2;
   public Ability3 Ability3;
   public Ability4 Ability4;

   public SetItem(uint itemCode)
   {
      switch (itemCode)
      {
         case 1:
            NomalAttack = new NomalAttack();
            break;
         case 2:
            Attack1 = new Attack1();
            break;
         case 3:
            Attack2 = new Attack2();
            break;
         case 4:
            Attack3 = new Attack3();
            break;
         case 5:
            Attack4 = new Attack4();
            break;
         case 6:
            NomalDefense = new NomalDefense();
            break;
         case 7:
            Defense1 = new Defense1();
            break;
         case 8:
            Defense2 = new Defense2();
            break;
         case 9:
            Defense3 = new Defense3();
            break;
         case 10:
            Defense4 = new Defense4();
            break;
         case 11:
            NomalAbility = new NomalAbility();
            break;
         case 12:
            Ability1 = new Ability1();
            break;
         case 13:
            Ability2 = new Ability2();
            break;
         case 14:
            Ability3 = new Ability3();
            break;
         case 15:
            Ability4 = new Ability4();
            break;
      }
   }
}
