using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    /// <summary>
    /// 반복 재생되는 하는 브금
    /// </summary>
    Bgm,
    
    /// <summary>
    /// 한번씩만 재생되는 효과음
    /// </summary>
    Effect,
    
    /// <summary>
    /// enum의 개수를 체크하기 위해 추가
    /// </summary>
    MaxCount, 
}

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField]
    private SetSounds _setSounds;
    private AudioSource[] _audioSources = new AudioSource[(int) SoundType.MaxCount];

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        GameObject root = GameObject.Find("SoundManager");

        string[] soundNames = System.Enum.GetNames(typeof(SoundType)); // "Bgm", "Effect"

        for (int i = 0; i < soundNames.Length - 1; i++)
        {
            GameObject go = new GameObject {name = soundNames[i]};
            _audioSources[i] = go.AddComponent<AudioSource>();
            go.transform.parent = root.transform;
        }

        _audioSources[(int) SoundType.Bgm].loop = true; // bgm 재생기는 무한 반복 재생
    }

    /// <summary>
    /// bgm 재생
    /// </summary>
    public void PlayBGM(BGMType soundName, float pitch = 1.0f)
    {
        AudioClip audioClip = GetBGMClip(soundName);
        PlayBgmOrEffect(audioClip, SoundType.Bgm, pitch);
    }
    
    /// <summary>
    /// 효과음 재생
    /// </summary>
    public void PlayEffect(EffectType soundName, float pitch = 1.0f)
    {
        AudioClip audioClip = GetEffectClip(soundName);
        PlayBgmOrEffect(audioClip, SoundType.Effect, pitch);
    }
    
    
    private void PlayBgmOrEffect(AudioClip audioClip, SoundType type, float pitch = 1.0f)
    {
        if (audioClip == null)
            return;

        if (type == SoundType.Bgm) // BGM 배경음악 재생
        {
            Debug.Log(_audioSources);
            AudioSource audioSource = _audioSources[(int) SoundType.Bgm];
            Debug.Log(_audioSources.Length);
            if (audioSource.isPlaying) //BGM은 중첩되면 안되기에 재생중인게 있다면 정지
                audioSource.Stop();

            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else if (type == SoundType.Effect) // Effect 효과음 재생
        {
            AudioSource audioSource = _audioSources[(int) SoundType.Effect];
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
    }

    private AudioClip GetBGMClip(BGMType bgm)
    {
        AudioClip clip = null;
        switch (bgm)
        {
            case BGMType.Lobby:
                clip = _setSounds.LobbyBGM;
                break;
            case BGMType.Select:
                clip = _setSounds.SelectBGM;
                break;
            case BGMType.InGame:
                clip = _setSounds.InGameBGM;
                break;
        }

        return clip;
    }
    
    private AudioClip GetEffectClip(EffectType bgm)
    {
        AudioClip clip = null;
        switch (bgm)
        {
            case EffectType.Button:
                clip = _setSounds.ButtonEffect;
                break;
            case EffectType.Click:
                clip = _setSounds.ClickEffect;
                break;
        }

        return clip;
    }
    
    
    /// <summary>
    /// 게임이 오래 지속 될때 새로운 사운드가 계속 추가되어 많아져서 한번 초기화가 필요할때 쓰이는 함수
    /// </summary>
    public void Clear()
    {
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
    }
}