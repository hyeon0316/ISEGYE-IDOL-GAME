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
        int id = 1;
        for (int i = 0; i < Battles.Length; i++)
        {
            Battles[i].SetPlayer(id++);
            Battles[i].SetEnemy(id);
            Battles[i].StartBattleCo();
        }
    }
}
