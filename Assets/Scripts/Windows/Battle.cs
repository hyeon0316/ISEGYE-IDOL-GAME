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

    private void OnEnable()
    {
        SetPlayer();
        SetEnemy();
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

    public void StartBattle()
    {
        BattlePlayers[0].ActiveItem();
        BattlePlayers[1].ActiveItem();
    }

    
    private void SetPlayer()
    {
        BattlePlayers[0].SetBattlePlayer(PlayerManager.Instance.Players[0], PlayerManager.Instance.Players[0].GetRandItemOrder());
    }

    private void SetEnemy()
    {
        int rand = Random.Range(1, 8);
        BattlePlayers[1].SetBattlePlayer(PlayerManager.Instance.Players[rand], PlayerManager.Instance.Players[rand].GetRandItemOrder());
    }
}
