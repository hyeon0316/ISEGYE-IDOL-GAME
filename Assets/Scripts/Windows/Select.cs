using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Profiling.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine.UI;

public class Select : MonoBehaviour
{
    [Header("CharacterInfo")] 
    public GameObject InfoWindow;
    public Image CharacterImage;
    public GameObject SelectButton;
    
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI DescText;

    private void Awake()
    {
        SetInfo(false);
    }

    private void SetInfo(bool isActive)
    {
        InfoWindow.SetActive(isActive);
        CharacterImage.gameObject.SetActive(isActive);
        SelectButton.SetActive(isActive);
    }
  
    public void ShowInfo(string name, string desc, Sprite image)
    {
        NameText.text = name;
        DescText.text = desc;
        CharacterImage.sprite = image;
        
        SetInfo(true);
    }

    /// <summary>
    /// 자신이 선택한 캐릭터 생성
    /// </summary>
    public void CreateCharacter()
    {
        var obj = new GameObject("Player");
        obj.AddComponent<Player>();
        obj.GetComponent<Player>().SetStat(1, 100, 10, NameText.text);
        
        WindowManager.Instance.SetWindow((int)WindowType.InGame);
        CreatePlayer();
    }

    /// <summary>
    /// 자신을 제외한 나머지 7명의 플레이어 생성
    /// </summary>
    public void CreatePlayer()
    {
        for (int i = 0; i < 7; i++)
        {
            var obj = new GameObject("Player");
            obj.AddComponent<Player>();
            obj.GetComponent<Player>().SetStat(i + 2, 100, 10, NameText.text);
        }
    }
    
    
}
