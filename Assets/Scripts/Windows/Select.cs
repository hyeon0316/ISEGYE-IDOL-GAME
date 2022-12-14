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
                PlayerManager.Instance.GetPlayer(networkID).Type = (ECharacterType)characterType;
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


    /// <summary>
    /// 버튼 활성화 비활성화 여부
    /// </summary>
    private void SetButton(bool isActive)
    {
        foreach (var btn in Characters)
        {
            btn.GetComponent<Button>().interactable = isActive;
        }
    }

    /// <summary>
    /// 하나의 버튼 외에 나머지를 활성화
    /// </summary>
    private void SetOnlyButton(int type)
    {
        foreach (var btn in Characters)
        {
            if(btn.CurCharType == (ECharacterType)type)
                btn.GetComponent<Button>().interactable = false;
            else
                btn.GetComponent<Button>().interactable = true;
        }
    }

    /// <summary>
    /// 캐릭터를 직접 선택
    /// </summary>
    public void PickCharacterButton(int type)
    {
        Player player = PlayerManager.Instance.Players[0];
       
        NetworkManager.Instance.SendChangeCharacterPacket(player.ID, type);
        player.Type = (ECharacterType)type;
        
        SetOnlyButton(type);
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
        SetCharacter();
        yield return new WaitForSeconds(1f);
        Debug.Log("게임 시작");
        WindowManager.Instance.SetWindow((int)EWindowType.InGame);
        PlayerManager.Instance.AddDefaultItem();
    }

    /// <summary>
    /// 제한시간이 지난 이후 모든 플레이어들의 캐릭터 셋팅
    /// </summary>
    private void SetCharacter()
    {
        for (int i = 0; i < PlayerManager.Instance.Players.Length; i++)
        {
            if (PlayerManager.Instance.Players[i].Type == ECharacterType.Empty)//캐릭터를 고르지 않았을 경우
            {
                Character character = Characters[(int) ECharacterType.Woowakgood];

                ChoiceCharacters[i].Image.sprite = character.Image.sprite;
                PlayerManager.Instance.Players[i].Init(ChoiceCharacters[i].Image.sprite, 100, 10);
            }
            else//캐릭터를 고른 경우
            {
                Character character = Characters[(int)PlayerManager.Instance.Players[i].Type];
                
                ChoiceCharacters[i].Image.sprite = character.Image.sprite;
                PlayerManager.Instance.Players[i].Init(ChoiceCharacters[i].Image.sprite,100,10);
            }
            
        }
    }
}
