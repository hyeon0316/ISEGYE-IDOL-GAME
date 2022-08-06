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

    public void EntryGame()
    {
        WindowManager.Instance.SetWindow((int)WindowType.InGame);
    }
}
