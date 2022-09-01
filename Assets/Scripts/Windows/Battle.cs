using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    public BattlePlayer[] BattlePlayers;

    public bool IsFinish { get; set; }

    private int _count;
    
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
                BattlePlayers[0].DeleteItem();
                BattlePlayers[1].DeleteItem();
                FindObjectOfType<BattleManager>().CheckFinishBattle();
                break;
            }

            _count++;
            if (_count == Global.NextBattle)//두 플레이어 아이템 모두 사용 뒤 곧 이어 다음 배틀을 위한 준비 시간
            {
                _count = 1;
                BattlePlayers[0].UseNextItem();
                BattlePlayers[1].UseNextItem();
                yield return new WaitForSeconds(1f);
            }

            BattlePlayers[0].ActiveItem();
            BattlePlayers[1].ActiveItem();
        
            yield return new WaitForSeconds(2f);
        }
    }
    
    public void StartBattle()
    {
        IsFinish = false;
        _count = 0;
        StartCoroutine(StartBattleCo());
    }

    
    public void SetPlayer(int playerID)
    {
        Player player = PlayerManager.Instance.GetPlayer(playerID);
        BattlePlayers[0].SetBattlePlayer(player, player.ActiveIndex, BattlePlayers[1]);
    }

    public void SetEnemy(int enemyID)
    {
        Player enemy = PlayerManager.Instance.GetPlayer(enemyID);
        BattlePlayers[1].SetBattlePlayer(enemy, enemy.ActiveIndex, BattlePlayers[0]);
    }
    
}
