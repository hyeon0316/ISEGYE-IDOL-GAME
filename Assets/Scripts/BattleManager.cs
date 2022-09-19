using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BattleManager : Singleton<BattleManager>
{
    public Battle[] Battles;

    public List<Int32> BattleOpponents = new List<Int32>();

    
    private void OnEnable()
    {
        SetBattle();
        StartBattle();
        SetBattleView(PlayerManager.Instance.Players[0].ID);
    }

    private void SetBattle()
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
            Battles[i].SetFirstPlayer(BattleOpponents[index++]);
            Battles[i].SetEnemy(BattleOpponents[index] < 0 ? ~BattleOpponents[index] : BattleOpponents[index]);
            index++;
        }
    }

    private void StartBattle()
    {
        foreach (var battle in Battles)
        {
            battle.StartBattle();
        }
    }

    /// <summary>
    /// 남은 플레이어 중 카메라에 담길 전투화면 셋팅
    /// </summary>
    public void SetBattleView(int playerID)
    {
        foreach (var battle in Battles)
        {
            battle.gameObject.transform.position = new Vector3(0, 1200, 0);
            foreach (var battlePlayer in battle.BattlePlayers)
            {
                if (battlePlayer.Player.ID == playerID)
                {
                    battle.gameObject.transform.position = new Vector3(-128, 0, 0);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// 남아있는 전체 플레이어들의 전투가 끝났는지 체크
    /// </summary>
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
