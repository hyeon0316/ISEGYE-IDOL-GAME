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
    
    private PlayerManager _playerManager;
    
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

    private void Awake()
    {
        _playerManager = FindObjectOfType<PlayerManager>();
        
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

    
    private void SetPlayer()
    {
       _playerManager.Players[_playerManager.PlayerID - INDEX].SetItem(_itemSlots);
       _playerImage.sprite = _playerManager.Players[_playerManager.PlayerID - INDEX].Sprite;
       _debugPlayerID.text = $"{_playerManager.Players[_playerManager.PlayerID - INDEX].ID}";
    }

    private void SetEnemy()
    {
        int rand = Random.Range(1, 8);
        _playerManager.Players[rand].SetItem(_eItemSlots);
        _debugEnemyID.text = $"{_playerManager.Players[rand].ID}";
    }
}
