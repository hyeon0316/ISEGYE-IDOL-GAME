using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
   public int ID { get; set; }
   public TextMeshProUGUI NickNameText;
   public TextMeshProUGUI HpText;
   public Image CharacterImage;

   /// <summary>
   /// 클릭시 해당 플레이어의 현황을 볼수 있음
   /// </summary>
   public void ShowPlayersInfo() //todo: 수정(임시방편)
   {
       if (InGame.CurGameType == GameType.Ready)
       {
           PlayerManager.Instance.SetPlayerView(ID);
       }
       else if (InGame.CurGameType == GameType.Battle)
       {
           BattleManager.Instance.SetBattleView(ID);
       }
   }
}
