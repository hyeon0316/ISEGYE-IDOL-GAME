using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    public BattlePlayer[] BattlePlayers;

    public bool IsFinish;
    
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
            if (BattlePlayers[0].AvatarHp <= 0 || BattlePlayers[1].AvatarHp <= 0) //todo: 나중에는 제한시간이 다 지나면 끝나는 조건문도 추가
            {
                IsFinish = true;
                FindObjectOfType<BattleManager>().CheckFinishBattle();
                break;
            }

            BattlePlayers[0].ActiveItem();
            BattlePlayers[1].ActiveItem();
        
            BattlePlayers[1].UpdateAvatarHp(-20);//임시
            yield return new WaitForSeconds(2f);
        }
    }
    
    public void StartBattle()
    {
        StartCoroutine(StartBattleCo());
    }

    
    public void SetPlayer(int playerIndex)
    {
        BattlePlayers[0].SetBattlePlayer(PlayerManager.Instance.Players[playerIndex], PlayerManager.Instance.Players[playerIndex].ActiveIndex);
        BattlePlayers[0].SetFirstTurn();//순서 임시 부여
    }

    public void SetEnemy(int enemyIndex)
    {
        BattlePlayers[1].SetBattlePlayer(PlayerManager.Instance.Players[enemyIndex], PlayerManager.Instance.Players[enemyIndex].ActiveIndex);
    }
    
}
