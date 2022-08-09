using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerManager : MonoBehaviour
{
   public Transform EnemyParent;
   private const int PLAYER_COUNT = 8;
   private const int INDEX = 1;
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

   public void CreatePlayer()
   {
      Player  player = GameObject.Find("Canvas").transform.Find("InGame").transform.Find("Background").transform.Find("Ready").GetComponent<Player>();

      player.SetID(_playerID);
      Players[_playerID - INDEX] = player;
      
      CreateEnemy(_playerID);
      
   }

   private void CreateEnemy(int exceptID)
   {
      _enemyIndex = 1;
      for (int i = 1; i <= PLAYER_COUNT; i++)
      {
         if(exceptID == i)
            continue;

         GameObject enemy = Instantiate(Resources.Load<GameObject>("Prefabs/Enemy"));
         enemy.transform.SetParent(EnemyParent);
         enemy.name = $"Enemy{i}";
         enemy.GetComponent<Player>().SetID(i);

         Players[_enemyIndex] = enemy.GetComponent<Player>();
         _enemyIndex++;
      }
   }
   
   
}
