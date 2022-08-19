using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Battle : MonoBehaviour
{
    public BattlePlayer[] BattlePlayers;

    public byte[] EnemyActiveIndex = new byte[60];
    
    private void OnEnable()
    {
        SetPlayer();
        SetEnemy();
        StartCoroutine(StartBattleCo());
    }

    private void FinishBattle()
    {
        //todo: 나중에는 모든 플레이어가 전투가 끝나면 최종적으로 종료(Ready창으로 이동)
    }

    public void SetFirstPlayer(int networkID)
    {
        if (BattlePlayers[0].Player.ID == networkID)
            BattlePlayers[0].SetFirstTurn();
        else
            BattlePlayers[1].SetFirstTurn();
    }

    private IEnumerator StartBattleCo()
    {
        while (true)
        {
            if (BattlePlayers[0].AvatarHp <= 0 || BattlePlayers[1].AvatarHp <= 0) //todo: 나중에는 제한시간이 다 지나면 끝남
            {
                FindObjectOfType<InGame>().CloseBattle();//임시(플레이어 8명 다 같이 끝나고 넘어가야함)
                break;
            }

            StartBattle();
            yield return new WaitForSeconds(2f);
        }
    }
    
    public void StartBattle()
    {
        BattlePlayers[0].ActiveItem();
        BattlePlayers[1].ActiveItem();
        
        BattlePlayers[1].UpdateAvatarHp(-20);//임시
    }

    
    private void SetPlayer()
    {
        BattlePlayers[0].SetBattlePlayer(PlayerManager.Instance.Players[0], PlayerManager.Instance.Players[0].GetRandItemOrder());
        BattlePlayers[0].SetFirstTurn();//순서 임시 부여
    }

    private void SetEnemy()
    {
        Player enemy = PlayerManager.Instance.Players[1];
        enemy.ActiveIndex = EnemyActiveIndex;        
        BattlePlayers[1].SetBattlePlayer(enemy, enemy.ActiveIndex);
    }
}
