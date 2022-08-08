using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ServerRoom : MonoBehaviour
{
   public Transform EnemyParent;
   private const int PLAYER_COUNT = 8;
   private int _index;
   
   private Select _select;

   public Player Player;
   public Player[] Enemy = new Player[7];

   private void Awake()
   {
      _select = GameObject.Find("Canvas").transform.Find("Select").GetComponent<Select>();
   }


   public void CreatePlayer()
   {
      int playerID = Random.Range(1, 9);
      Player  player = GameObject.Find("Canvas").transform.Find("InGame").transform.Find("Background").transform.Find("Ready").GetComponent<Player>();

      player.SetStat(playerID, _select.CharacterImage.sprite, 100, 10, "Player");
      Player = player;
      
      CreateEnemy(playerID);
      
   }

   private void CreateEnemy(int exceptID)
   {
      _index = 0;
      for (int i = 1; i <= PLAYER_COUNT; i++)
      {
         if(exceptID == i)
            continue;

         GameObject enemy = Instantiate(Resources.Load<GameObject>("Prefabs/Enemy"));
         enemy.transform.SetParent(EnemyParent);
         enemy.name = $"Enemy{i}";
         enemy.GetComponent<Player>().SetStat(i, null, 100,10, "AI");

         Enemy[_index] = enemy.GetComponent<Player>();
         _index++;
      }
   }
   
   
}
