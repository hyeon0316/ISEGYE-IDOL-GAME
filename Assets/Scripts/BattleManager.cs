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

    public Int32[] BattleOpponents;

    
    private void OnEnable()
    {
        StartBattle();
    }

    private void StartBattle()
    {
        int index = 0;
        for (int i = 0; i < Battles.Length; i++)
        {
            Battles[i].SetPlayer(BattleOpponents[index++]);
            Battles[i].SetEnemy(BattleOpponents[index++]);
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
