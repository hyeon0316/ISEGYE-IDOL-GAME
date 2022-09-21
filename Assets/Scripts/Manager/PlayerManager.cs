using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
   public Transform EnemyParent;
   public const int PLAYER_COUNT = 8;
   private Select _select;

   public Player[] Players = new Player[8];
   

   private void Awake()
   {
      _select = GameObject.Find("Canvas").transform.Find("Select").GetComponent<Select>();
   }

   public Player GetPlayer(int networkID)
   {
      foreach(var player in Players)
      {
         if(player.ID == networkID)
            return player;
      }
      return null;
   }

   public void AutoSetItem()
   {
      foreach (var player in Players)
      {
         player.AutoSetItem();
      }
   }

   /// <summary>
   /// 남은 플레이어 중 카메라 안에 담길 플레이어를 셋팅
   /// </summary>
   public void SetPlayerView(int playerID)
   {
      foreach (var player in PlayerManager.Instance.Players)
      {
         if (player.ID == playerID)
            player.gameObject.transform.position = new Vector3(0, 0, 0);
         else
            player.gameObject.transform.position = new Vector3(0, 1200, 0);
      }
   }

   public void AddDefaultItem()
   {
      foreach (var player in Players)
      {
         player.AddDefaultItem();
      }
   }

   public void CreateEnemy(Packet.UserInfo[] userInfos)
   {
      int index = 1;
      for (int i = 0; i < PLAYER_COUNT; i++)
      {
         if (userInfos[i].networkID == Players[0].ID)
         {
            continue;
         }
         
         GameObject enemy = Instantiate(Resources.Load<GameObject>("Prefabs/Enemy"));
         enemy.transform.SetParent(EnemyParent);
         enemy.transform.position += new Vector3(0, 1200, 0);
         enemy.name = $"Enemy{userInfos[i].networkID}";
         Players[index] = enemy.GetComponent<Player>();
         Players[index].SetID((int)userInfos[i].networkID);
         Players[index].SetName(Encoding.Unicode.GetString(userInfos[i].name));
         Debug.Log(Players[index].NickName);
         ++index;
      }
   }
}
