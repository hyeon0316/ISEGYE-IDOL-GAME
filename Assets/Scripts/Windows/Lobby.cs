using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : MonoBehaviour
{
    //todo: 지금은 로비에 Start버튼 뿐이지만 나중에 다른 기능 추가
    public GameObject NickNameParent;
    public TMP_InputField InputNickName;
    public GameObject StartButton;

    public void OnInputField()
    {
        NickNameParent.SetActive(true);
        StartButton.SetActive(false);
    }

    public void SetNickName()
    {
        if (InputNickName.text.Trim((char) 8203).Length == 0 || string.IsNullOrWhiteSpace(InputNickName.text))
            InputNickName.text = "플레이어";
        
        PlayerManager.Instance.Players[0].SetName(InputNickName.text);
        WindowManager.Instance.SetWindow((int)EWindowType.Server);
    }

   
}
