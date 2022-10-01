using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BGMType
{
    Lobby,
    Select,
    InGame
}

public enum EffectType
{
    Button,
    Click
}
public class SetSounds : MonoBehaviour
{
    [Header("배경음")]
    public AudioClip LobbyBGM;
    public AudioClip SelectBGM;
    public AudioClip InGameBGM;
    
    [Header("효과음")]
    public AudioClip ButtonEffect;
    public AudioClip ClickEffect;
}
