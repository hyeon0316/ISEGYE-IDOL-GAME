using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Battle : MonoBehaviour
{
    private const int INDEX = 1;
    
    //플레이어
    public GameObject MyFiled;
    private GameObject _usingItems;
    private ItemSlot[] _itemSlots;
    private Image _playerImage;

    //적
    public GameObject EnemyFiled;
    private GameObject _eUsingItems;
    private ItemSlot[] _eItemSlots;
    private Image _enemyImage;

    private TextMeshProUGUI _debugPlayerID;
    private TextMeshProUGUI _debugEnemyID;

    public BattlePlayer[] BattlePlayers;

    private void Awake()
    {
        _usingItems = MyFiled.transform.GetChild(0).transform.gameObject;
        _playerImage = MyFiled.transform.GetChild(1).GetComponent<Image>();
        _itemSlots = _usingItems.GetComponentsInChildren<ItemSlot>();
        
        _eUsingItems = EnemyFiled.transform.GetChild(0).transform.gameObject;
        _enemyImage = EnemyFiled.transform.GetChild(1).GetComponent<Image>();
        _eItemSlots = _eUsingItems.GetComponentsInChildren<ItemSlot>();

        _debugPlayerID = _playerImage.GetComponentInChildren<TextMeshProUGUI>();
        _debugEnemyID = _enemyImage.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        SetPlayer();
        SetEnemy();
    }

    public void SetFirstPlayer(int networkID)
    {
        if (BattlePlayers[0].Player.ID == networkID)
            BattlePlayers[0].SetFirstTurn();
        else
            BattlePlayers[1].SetFirstTurn();
    }

    public void BattleAAA()
    {
        BattlePlayers[0].ActiveItem();
        BattlePlayers[1].ActiveItem();
    }

    private void Foo()
    {
        //BattlePlayers[0].SetBattlePlayer(PlayerManager.Instance.Players[0], PlayerManager.Instance.Players[0].GetRandItemOrder());
    }
    
    private void SetPlayer()
    {
        PlayerManager.Instance.Players[0].SetItem(_itemSlots);
       _playerImage.sprite = PlayerManager.Instance.Players[0].Sprite;
       _debugPlayerID.text = $"{PlayerManager.Instance.Players[0].ID}";
    }

    private void SetEnemy()
    {
        int rand = Random.Range(1, 8);
        PlayerManager.Instance.Players[rand].SetItem(_eItemSlots);
        _enemyImage.sprite = PlayerManager.Instance.Players[rand].Sprite;
        _debugEnemyID.text = $"{PlayerManager.Instance.Players[rand].ID}";
    }
}
