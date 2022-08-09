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
    public Image[] CharacterImage;
    
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI DescText;
    public TextMeshProUGUI TimerText;

    private float _timer;

    private PlayerManager _playerManager;


    public Character[] Characters;

    private void Awake()
    {
        _playerManager = FindObjectOfType<PlayerManager>();
    }


    private void OnEnable()
    {
        _playerManager.CreatePlayer();
        SetInfo(false);
        SetButton(true);
        StartCoroutine(SetSelectTimerCo());
    }

    private void SetInfo(bool isActive)
    {
        InfoWindow.SetActive(isActive);
    }
  
    public void ShowInfo(string name, string desc, Sprite image)
    {
        NameText.text = name;
        DescText.text = desc;
        CharacterImage[0].sprite = image;
        
        SetInfo(true);
    }


    private void SetButton(bool isActive)
    {
        foreach (var btn in Characters)
        {
            btn.GetComponent<Button>().interactable = isActive;
        }
    }
    
    
    public void EntryInGame()
    {
        StopCoroutine(SetSelectTimerCo());
        WindowManager.Instance.SetWindow((int)WindowType.InGame);
    }

    /// <summary>
    /// 캐릭터를 직접 선택
    /// </summary>
    public void PickCharacterButton()
    {
        _playerManager.Players[_playerManager.PlayerID - 1].SetStat(CharacterImage[0].sprite, 100, 10, NameText.text);
    }
    
    private IEnumerator SetSelectTimerCo()
    {
        _timer = 15;
        while (true)
        {
            if (_timer <= 0)
            {
                _timer = 0;
                TimerText.text = $"남은 시간: {_timer}";
                break;
            }

            TimerText.text = $"남은 시간: {Mathf.Ceil(_timer)}";
            _timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        SetButton(false);
        SetAutoCharacter();
        yield return new WaitForSeconds(1f);
        Debug.Log("게임 시작");
        WindowManager.Instance.SetWindow((int)WindowType.InGame);
    }

    /// <summary>
    /// 캐릭터를 고르지 않은 상태에서 제한시간이 지날 때
    /// </summary>
    private void SetAutoCharacter()
    {
        for (int i = 0; i < CharacterImage.Length; i++)
        {
            if (CharacterImage[i].sprite == null)
            {
                Character character = Characters[(int) Character.CharacterType.Woowakgood];

                CharacterImage[i].sprite = character.Image.sprite;
                _playerManager.Players[i].SetStat(CharacterImage[i].sprite, 100, 10, character.Name);
            }
        }
    }
}
