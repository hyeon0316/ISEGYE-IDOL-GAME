using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Battle : MonoBehaviour
{
    private const int INDEX = 1;
    
    private ServerRoom _serverRoom;
    
    //플레이어
    public GameObject MyFiled;
    private GameObject _usingItems;
    private ItemSlot[] _itemSlots;
    private Image _playerImage;
    private Player _player;
    
    //적
    public GameObject EnemyFiled;
    private GameObject _eUsingItems;
    private ItemSlot[] _eItemSlots;
    private Image _enemyImage;
    private Player _enemy;

    private void Awake()
    {
        _serverRoom = FindObjectOfType<ServerRoom>();
        
        _usingItems = MyFiled.transform.GetChild(0).transform.gameObject;
        _playerImage = MyFiled.transform.GetChild(1).GetComponent<Image>();
        _itemSlots = _usingItems.GetComponentsInChildren<ItemSlot>();
        
        _eUsingItems = EnemyFiled.transform.GetChild(0).transform.gameObject;
        _enemyImage = EnemyFiled.transform.GetChild(1).GetComponent<Image>();
        _eItemSlots = _eUsingItems.GetComponentsInChildren<ItemSlot>();
    }

    private void OnEnable()
    {
        SetPlayer();
        SetEnemy();
        StartCoroutine(ActiveItemCo());
    }

    private void SetPlayer()
    {
        _player = _serverRoom.PlayerObjs[_serverRoom.PlayerID - INDEX];
        _player.SetItem(_itemSlots);
        _playerImage.sprite = _player.Sprite;
    }

    private void SetEnemy()
    {
        int randID = Random.Range(1, 9);

        while (true)
        {
            _enemy = _serverRoom.PlayerObjs[randID - INDEX];
            _enemy.SetItem(_eItemSlots);
            _enemyImage.sprite = _enemy.Sprite;
            
            if (randID == _serverRoom.PlayerID)
                randID = Random.Range(1, 9);
            else
                break;
        }
    }

    private IEnumerator ActiveItemCo()
    {
        for (int i = 0; i < _itemSlots.Length; i++)
        {
            _itemSlots[i].ActiveItem();
            yield return new WaitForSeconds(1f);
        }
    }
}
