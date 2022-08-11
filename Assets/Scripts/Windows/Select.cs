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
    public ChoiceCharacter[] ChoiceCharacters;
    
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI DescText;
    public TextMeshProUGUI TimerText;

    private float _timer;


    public Character[] Characters;

    
    private void OnEnable()
    {
        SetInfo(false);
        SetButton(true);
        StartCoroutine(SetSelectTimerCo());
    }

    private void SetInfo(bool isActive)
    {
        InfoWindow.SetActive(isActive);
    }

    public void SetUserInfo(Packet.UserInfo[] userInfos)
    {
        int index = 1;
        for (int i = 0; i < PlayerManager.PLAYER_COUNT; i++)
        {
            if (userInfos[i].networkID == ChoiceCharacters[0].NetworkID)
            {
                continue;
            }

            ChoiceCharacters[index++].NetworkID = (int)userInfos[i].networkID;
        }
    }
    
    public void ChangeCharacterImage(int networkID, int characterType)
    {
        for (int i = 0; i < ChoiceCharacters.Length; i++)
        {
            if (networkID == ChoiceCharacters[i].NetworkID)
            {
                ChoiceCharacters[i].ChangeCharacterImage(characterType);
                PlayerManager.Instance.GetPlayer(networkID).Type = (CharacterType)characterType;
                PlayerManager.Instance.GetPlayer(networkID).Sprite = ChoiceCharacters[i].Image.sprite;
                break;
            }
        }
    }

    public void ShowInfo(string name, string desc, Sprite image)
    {
        NameText.text = name;
        DescText.text = desc;
        ChoiceCharacters[0].Image.sprite = image;
        
        SetInfo(true);
    }


    private void SetButton(bool isActive)
    {
        foreach (var btn in Characters)
        {
            btn.GetComponent<Button>().interactable = isActive;
        }
    }
    
    

    /// <summary>
    /// 캐릭터를 직접 선택
    /// </summary>
    public void PickCharacterButton(int type)
    {
        PlayerManager.Instance.Players[0].SetStat(ChoiceCharacters[0].Image.sprite, 100, 10);
        NetworkManager.Instance.SendChangeCharacterPacket(PlayerManager.Instance.Players[0].ID, type);
        PlayerManager.Instance.Players[0].Type = (CharacterType)type;
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
        for (int i = 0; i < ChoiceCharacters.Length; i++)
        {
            if (ChoiceCharacters[i].Image.sprite == null)
            {
                Character character = Characters[(int) CharacterType.Woowakgood];

                ChoiceCharacters[i].Image.sprite = character.Image.sprite;
                PlayerManager.Instance.Players[i].SetStat(ChoiceCharacters[i].Image.sprite, 100, 10);
            }
        }
    }
}
