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
    public List<int> RandPercent = new List<int>();
    
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
        StartCoroutine(ActiveItemCo());
    }

    
    private void SetPlayer()
    {
       _playerManager.Players[_playerManager.PlayerID].SetItem(_itemSlots);
       _playerImage.sprite = _playerManager.Players[_playerManager.PlayerID].Sprite;
       _debugPlayerID.text = $"{_playerManager.Players[_playerManager.PlayerID].ID}";
    }

    private void SetEnemy()
    {
        int rand = Random.Range(2, 9);
        _playerManager.Players[rand].SetItem(_eItemSlots);
        _debugEnemyID.text = $"{_playerManager.Players[rand].ID}";
    }

    private IEnumerator ActiveItemCo()
    {
        float rand;
        List<int> saveIndex = new List<int>();

        for (int i = 0; i < _itemSlots.Length; i++)
        {
            saveIndex.Add(i);
        }

        while (true)
        {
            if (saveIndex.Count == 0)
                break;
            
            for (int i = 0; i < saveIndex.Count; i++)
            {
                rand = Random.Range(1, 100);
                
                if (rand <= RandPercent[saveIndex[i]])
                {
                    yield return new WaitForSeconds(2f);
                    _itemSlots[saveIndex[i]].ActiveItem();
                    saveIndex.RemoveAt(i);
                    break;
                }
                else
                {
                    continue;
                }
            }

        }
        

    }
}
