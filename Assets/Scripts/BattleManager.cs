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

    
    private void OnEnable()
    {
        int index = 0;
        for (int i = 0; i < Battles.Length; i++)
        {
            Battles[i].SetPlayer(index++);
            Battles[i].SetEnemy(index++);
            Battles[i].StartBattle();
        }
    }

    private void Update()
    {
       
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
