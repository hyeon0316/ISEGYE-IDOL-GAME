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

    public string NickName;

    private Sprite _sprite;
    public Sprite Sprite => _sprite;
    
    [SerializeField]
    private int _id;

    public int ID => _id;
    public int Hp => _hp;

    private UsingInventory _usingInventory;
    private UnUsingInventory _unUsingInventory;

    private void Awake()
    {
        _usingInventory = new UsingInventory(); //플레이어 마다 서로 각자의 인벤토리를 가지고 있어야 하므로 동적생성
        _unUsingInventory = new UnUsingInventory();
    }

    public void SetInventory()
    {
        _usingInventory = GameObject.Find("Canvas").transform.Find("InGame").transform.Find("Background").transform
            .Find("Ready").transform.Find("UsingInventory").GetComponent<UsingInventory>();
        
        _unUsingInventory = GameObject.Find("Canvas").transform.Find("InGame").transform.Find("Background").transform
            .Find("Ready").transform.Find("UnUsingInventory").GetComponent<UnUsingInventory>();
    }

    public void SetEnemyInventory(GameObject enemyInven)
    {
        _usingInventory = enemyInven.transform.GetChild(0).GetComponent<UsingInventory>();
        _unUsingInventory = enemyInven.transform.GetChild(1).GetComponent<UnUsingInventory>();
    }
    
    public void SetStat(int id,Sprite image, int hp, int defense, string name)
    {
        _id = id;
        _sprite = image;
        _hp = hp;
        _defense = defense;
        NickName = name;
    }

    /// <summary>
    /// 준비단계에서 셋팅한 아이템을 전투단계에 사용
    /// </summary>
    public void SetItem(ItemSlot[] usingSlots)
    {
        for (int i = 0; i < _usingInventory.ItemSlots.Length; i++)
        {
            if (_usingInventory.ItemSlots[i].transform.childCount == 1)
            {
                usingSlots[i].AddNewItem(_usingInventory.ItemSlots[i].GetComponentInChildren<Item>().Code);
            }
        }
    }
}
