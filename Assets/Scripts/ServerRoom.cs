using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ServerRoom : MonoBehaviour
{
   private const int PLAYER_COUNT = 8;
   private const int INDEX = 1;

   private int _playerID;

   public int PlayerID => _playerID;
   
   public Player[] PlayerObjs;

   private Select _select;

   public GameObject[] EnemyInvens;
   
   private void Awake()
   {
      _select = GameObject.Find("Canvas").transform.Find("Select").GetComponent<Select>();
      
      for (int i = 0; i < PLAYER_COUNT; i++)
      {
         var playerObj = new GameObject($"Player{i + 1}");
         playerObj.transform.SetParent(this.transform);
         playerObj.AddComponent<Player>();
      }
      
      PlayerObjs = GetComponentsInChildren<Player>();
   }

   
   public void CreatePlayer()
   {
      _playerID = Random.Range(1, 9);
      
      var player = PlayerObjs[_playerID - INDEX];
      player.SetInventory();
      player.SetStat(_playerID,_select.CharacterImage.sprite, 100, 10, _select.NameText.text);
      Debug.Log($"플레이어{_playerID} 체력:{player.Hp}");
      
      CreateEnemy(_playerID);
   }

   private void CreateEnemy(int exceptID)
   {
      int invenIndex = 0;
      for (int i = 0; i < PLAYER_COUNT; i++)
      {
         if (i == exceptID - INDEX)
            continue;
         
         var enemy = PlayerObjs[i];
         enemy.SetEnemyInventory(EnemyInvens[invenIndex]);
         enemy.SetStat(i + INDEX, null, 100, 10, null);
         Debug.Log($"적{i + INDEX} 체력:: {enemy.Hp}");
         invenIndex++;
      }
   }
   
   
}
