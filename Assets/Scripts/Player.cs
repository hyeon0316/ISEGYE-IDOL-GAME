using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    private int _hp;
    private int _defense;

    public string Name;
    
    [SerializeField]
    private int _id;

    public int ID => _id;

    private UsingInventory _usingInventory;
    private UnUsingInventory _unUsingInventory;

    private void Awake()
    {
        _usingInventory = new UsingInventory(); //플레이어 마다 서로 각자의 인벤토리를 가지고 있어야 하므로 동적생성
        _unUsingInventory = new UnUsingInventory();
    }

    private void Start()
    {
        Debug.Log($"ID: {_id}");
        Debug.Log($"Hp: {_hp}");
        Debug.Log($"Defense: {_defense}");
    }
    
    public void SetStat(int id,int hp, int defense, string name)
    {
        _id = id;
        _hp = hp;
        _defense = defense;
        Name = name;
    }

    /// <summary>
    /// 100% 확률로 아이템 랜덤 획득(서버로 보낼 함수)
    /// </summary>
    public void AddItem(int itemCode)
    {
        _unUsingInventory.AddItem(itemCode);
    }

    /// <summary>
    /// 50%확률로 세가지 아이템 중 원하는 아이템 하나를 선택하여 획득
    /// </summary>
    private void SelectGetItem()
    {
        
    }

    
    /// <summary>
    /// 특정 아이템 하나를 강화 시도(실패 확률 있음)
    /// </summary>
    private void UpgradeItem(int itemCode)
    {
        
    }

}
