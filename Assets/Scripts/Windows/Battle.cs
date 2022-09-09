using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battle : MonoBehaviour
{
    public BattlePlayer[] BattlePlayers;
    public Image FinishImage;
    
    public bool IsFinish { get; set; }

    private int _count;
    
    public void SetFirstPlayer(int networkID)
    {
        if (BattlePlayers[0].Player.ID == networkID)
            BattlePlayers[0].SetFirstTurn();
        else
            BattlePlayers[1].SetFirstTurn();
    }

    /// <summary>
    /// 자신의 전투가 끝났는지 체크
    /// </summary>
    private bool FinishBattle(BattlePlayer player, BattlePlayer opponent)
    {
        if (player.AvatarHp <= 0 || opponent.AvatarHp <= 0)
        {
            if (player.AvatarHp <= 0)
                player.Player.UpdateHp(-40);
            else
                opponent.Player.UpdateHp(-40);

            IsFinish = true;
            player.DeleteItem();
            opponent.DeleteItem();
            BattleManager.Instance.CheckFinishBattle();
            //todo: 배틀 종료 표시 띄우기
            FinishImage.gameObject.SetActive(true);
            return true;
        }
        return false;
    }

    private IEnumerator StartBattleCo()
    {
        while (true)
        {
            if (FinishBattle(BattlePlayers[0],BattlePlayers[1])) //todo: 나중에는 제한시간이 다 지나면 끝나는 조건문도 추가
                break;

            _count++;
            if (_count == Global.NextBattle)//두 플레이어 아이템 모두 사용 뒤 곧 이어 다음 배틀을 위한 준비 시간
            {
                yield return new WaitForSeconds(1f);
                _count = 1;
                BattlePlayers[0].UseNextItem();
                BattlePlayers[1].UseNextItem();
            }
            
            yield return new WaitForSeconds(1f);
            BattlePlayers[0].ActiveItem();
            BattlePlayers[1].ActiveItem();
        }
    }
    
    public void StartBattle()
    {
        IsFinish = false;
        _count = 0;
        FinishImage.gameObject.SetActive(false);
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
