using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BattleManager : MonoBehaviour
{
    public Battle[] Battles;

    public List<Int32> BattleOpponents = new List<Int32>();

    
    
    private void OnEnable()
    {
        StartBattle();
    }

    private void StartBattle()
    {
        int mySelfID = 0;
        int opponentID = 0;

        //플레이어 자신과 자신의 상대 먼저 배틀 셋팅
        for (int i = 0; i < BattleOpponents.Count; i++)
        {
            if (BattleOpponents[i] == PlayerManager.Instance.Players[0].ID)
            {
                Battles[0].SetPlayer(BattleOpponents[i]);
                if (i % 2 == 1)
                {
                    Battles[0].SetEnemy(BattleOpponents[i + 1]);
                    Battles[0].SetFirstPlayer(BattleOpponents[i]);
                    BattleOpponents.Remove(BattleOpponents[i + 1]);
                }
                else
                {
                    Battles[0].SetEnemy(BattleOpponents[i - 1]);
                    Battles[0].SetFirstPlayer(BattleOpponents[i - 1]);
                    BattleOpponents.Remove(BattleOpponents[i - 1]);
                }
                BattleOpponents.Remove(BattleOpponents[i]);
                Battles[0].StartBattle();
                break;
            }
        }

        int index = 0;
        //나머지 다른 플레이어 배틀 셋팅
        for (int i = 1; i < Battles.Length; i++)
        {
            if (index == BattleOpponents.Count)
                break;
            
            Battles[i].SetPlayer(BattleOpponents[index]);
            Battles[i].SetFirstPlayer(BattleOpponents[index++]);
            Battles[i].SetEnemy(BattleOpponents[index++]);
            Battles[i].StartBattle();
        }
        
        
    }

    public void CheckFinishBattle()
    {
        foreach (var battle in Battles)
        {
            if (!battle.IsFinish)
                return;
        }
        
        FindObjectOfType<InGame>().CloseBattle();
    }
}
