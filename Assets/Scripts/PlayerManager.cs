using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerManager : Singleton<PlayerManager>
{
   public Transform EnemyParent;
   public const int PLAYER_COUNT = 8;
   private int _enemyIndex;

   private int _playerID = 1;
   public int PlayerID => _playerID;

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
         enemy.name = $"Enemy{userInfos[i].networkID}";
         Players[index] = enemy.GetComponent<Player>();
         Players[index++].SetID((int)userInfos[i].networkID);
      }
   }
}
