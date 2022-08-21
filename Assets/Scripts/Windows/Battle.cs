using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    public BattlePlayer[] BattlePlayers;
    
    
    public void SetFirstPlayer(int networkID)
    {
        if (BattlePlayers[0].Player.ID == networkID)
            BattlePlayers[0].SetFirstTurn();
        else
            BattlePlayers[1].SetFirstTurn();
    }
    
    public IEnumerator StartBattleCo()
    {
        while (true)
        {
            if (BattlePlayers[0].AvatarHp <= 0 || BattlePlayers[1].AvatarHp <= 0) //todo: 나중에는 제한시간이 다 지나면 끝남
            {
                //FindObjectOfType<InGame>().CloseBattle();
                //todo: 패킷 전송하여 모든 플레이어가 전투가 끝나면 Ready창 이동
                break;
            }

            StartBattle();
            yield return new WaitForSeconds(2f);
        }
    }
    
    private void StartBattle()
    {
        BattlePlayers[0].ActiveItem();
        BattlePlayers[1].ActiveItem();
        
        BattlePlayers[1].UpdateAvatarHp(-20);//임시
    }

    
    public void SetPlayer(int playerID)
    {
        BattlePlayers[0].SetBattlePlayer(PlayerManager.Instance.GetPlayer(playerID), PlayerManager.Instance.GetPlayer(playerID).ActiveIndex);
        BattlePlayers[0].SetFirstTurn();//순서 임시 부여
    }

    public void SetEnemy(int enemyID)
    {
        BattlePlayers[1].SetBattlePlayer(PlayerManager.Instance.GetPlayer(enemyID), PlayerManager.Instance.GetPlayer(enemyID).ActiveIndex);
    }
    
}
