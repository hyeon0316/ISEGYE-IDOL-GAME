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
        //플레이어 자신과 자신의 상대 먼저 배틀 셋팅
        int myselfID = 0;
        int opponentID = 0;
        for (int i = 0; i < BattleOpponents.Count; i++)
        {
            if (BattleOpponents[i] == PlayerManager.Instance.Players[0].ID)
            {
                Battles[0].SetPlayer(BattleOpponents[i]);
                myselfID = BattleOpponents[i];
                if ((i + 1) % 2 == 0)//적이 선 공격일때
                {
                    Battles[0].SetEnemy(BattleOpponents[i - 1] < 0 ? ~BattleOpponents[i - 1] : BattleOpponents[i - 1]);
                    Battles[0].SetFirstPlayer(BattleOpponents[i - 1]);
                    opponentID = BattleOpponents[i - 1];
                }
                else//자신이 선 공격일때
                {
                    Battles[0].SetEnemy(BattleOpponents[i + 1] < 0 ? ~BattleOpponents[i + 1] : BattleOpponents[i + 1]);
                    Battles[0].SetFirstPlayer(BattleOpponents[i]);
                    opponentID = BattleOpponents[i + 1];
                }
                Battles[0].StartBattle();
                break;
            }
        }

        int index = 0;
        //나머지 다른 플레이어 배틀 셋팅
        for (int i = 1; i < Battles.Length; i++)
        {
            if (BattleOpponents[index] == myselfID || BattleOpponents[index] == opponentID)
                index += 2;
                
            if (index == BattleOpponents.Count)
                break;

            Battles[i].SetPlayer(BattleOpponents[index] < 0 ? ~BattleOpponents[index] : BattleOpponents[index]);
            Battles[i].SetFirstPlayer(BattleOpponents[index++] < 0 ? ~BattleOpponents[index] : BattleOpponents[index]);
            Battles[i].SetEnemy(BattleOpponents[index++] < 0 ? ~BattleOpponents[index] : BattleOpponents[index]);
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
