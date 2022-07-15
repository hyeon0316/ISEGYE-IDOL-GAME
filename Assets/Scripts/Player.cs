using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private int _hp;
    private int _defense;

    public string Name;
    private uint _id;

    private UsingInventory _usingInventory;
    private UnUsingInventory _unUsingInventory;

    private void Awake()
    {
        _usingInventory = new UsingInventory(); //플레이어 마다 서로 각자의 인벤토리를 가지고 있어야 하므로 동적생성
        _unUsingInventory = new UnUsingInventory();
    }

    private void Start()
    {
        Debug.Log($"Hp: {_hp}");
        Debug.Log($"Defense: {_defense}");
    }
    
    public void SetStat(int hp, int defense, string name)
    {
        _hp = hp;
        _defense = defense;
        Name = name;
    }

    /// <summary>
    /// 아이템 끼리 자리를 변경
    /// </summary>
    private void SwapItem()
    {

    }

    private void AddNewItem(uint itemCode)
    {
        
    }

    /// <summary>
    /// 특정 아이템 하나를 강화 시도(실패 확률 있음)
    /// </summary>
    private void UpgradeItem(uint itemCode)
    {
        
    }

}
