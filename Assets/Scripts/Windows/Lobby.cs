using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : MonoBehaviour
{
    //todo: 지금은 로비에 Start버튼 뿐이지만 나중에 다른 기능 추가
    public GameObject InputNickName;
    public GameObject StartButton;

    public TextMeshProUGUI NickName;



    public void OnInputField()
    {
        InputNickName.SetActive(true);
        StartButton.SetActive(false);
    }

    public void SetNickName()
    {
        //todo: 닉네임 설정
        PlayerManager.Instance.Players[0].NickName = NickName.text;
        WindowManager.Instance.SetWindow((int)WindowType.Matching);
    }
}
