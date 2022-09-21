using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battle : MonoBehaviour
{
    public BattlePlayer[] BattlePlayers;
    public Image FinishImage;
    
    public bool IsFinish { get; set; }

    private int _count = 0;

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
    private bool IsFinishBattle(BattlePlayer player, BattlePlayer opponent)
    {
        if (player.AvatarHp <= 0 || opponent.AvatarHp <= 0)
        {
            if (player.AvatarHp <= 0)
                player.Player.UpdateHp(-40);
            else
                opponent.Player.UpdateHp(-40);

            IsFinish = true;
            InitItem(player, opponent);
            BattleManager.Instance.CheckFinishBattle();
            FinishImage.gameObject.SetActive(true);
            return true;
        }
        return false;
    }

    private void InitItem(BattlePlayer player, BattlePlayer opponent)
    {
        foreach (var itemSlot in player.ItemSlots)
            if(itemSlot.transform.childCount == 1)
                itemSlot.DeleteItem();
        
        foreach (var itemSlot in opponent.ItemSlots)
            if(itemSlot.transform.childCount == 1)
                itemSlot.DeleteItem();
    }
    
    public void StartBattle()
    {
        _count = -1;
        IsFinish = false;
        FinishImage.gameObject.SetActive(false);
        StartCoroutine(StartBattleCo());
    }
    
    private IEnumerator StartBattleCo()
    {
        while (true)
        {
            if (IsFinishBattle(BattlePlayers[0],BattlePlayers[1])) //todo: 나중에는 제한시간이 다 지나면 끝나는 조건문도 추가
                break;

            _count++;
            if (_count == Global.NextBattle)//다음 배틀을 위한 준비 시간
            {
                _count = 0;
                yield return new WaitForSeconds(1f);
                SetNextRound(BattlePlayers[0], BattlePlayers[1]);
                BattlePlayers[0].UseAttackCount = 0;
                BattlePlayers[1].UseAttackCount = 0;
            }

            yield return new WaitForSeconds(1f);
            
            BattlePlayers[0].ActiveItem();
            BattlePlayers[1].ActiveItem();
            
            //todo: 각 아이템이 발동할 시간 동안에는 다음 반복문으로 넘어가면 안됨
        }
    }

    /// <summary>
    ///  다음 아이템 순회 발동에 대한 준비
    /// </summary>
    private void SetNextRound(BattlePlayer player, BattlePlayer opponent)
    {
        foreach (var itemSlot in player.ItemSlots)
            if(itemSlot.transform.childCount == 1)
                itemSlot.ChangeColor(Color.white);
        
        foreach (var itemSlot in opponent.ItemSlots)
            if(itemSlot.transform.childCount == 1)
                itemSlot.ChangeColor(Color.white);
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
