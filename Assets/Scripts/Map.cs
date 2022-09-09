using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 오른쪽에 표시되는 플레이어들의 정보
/// </summary>
public class Map : MonoBehaviour
{
    public PlayerInfo[] PlayersInfo;

    
    public void SetPlayersInfo()
    {
        for (int i = 0; i < PlayersInfo.Length; i++)
        {
            PlayersInfo[i].ID = PlayerManager.Instance.Players[i].ID;
            PlayersInfo[i].NickNameText.text = PlayerManager.Instance.Players[i].NickName;
            PlayersInfo[i].HpText.text = $"{PlayerManager.Instance.Players[i].Hp}";
            PlayersInfo[i].CharacterImage.sprite = PlayerManager.Instance.Players[i].Sprite;
        }
    }

    public void UpdatePlayersHp(int playerID)
    {
        for (int i = 0; i < PlayersInfo.Length; i++)
        {
            if (PlayersInfo[i].ID == playerID)
            {
                PlayersInfo[i].HpText.text = $"{PlayerManager.Instance.Players[i].Hp}";
                break;
            }
        }
    }
}
