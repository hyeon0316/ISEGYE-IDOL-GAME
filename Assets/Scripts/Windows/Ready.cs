using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Ready : MonoBehaviour
{
    public GameObject RouletteWindow;
    public Button RouletteButton;
    
    public TextMeshProUGUI ReadyTimer;
    private float _readyTime;
    private bool _isActive = false;

    public float ReadyTime => _readyTime;
    
    private void OnEnable()
    {
        StartCoroutine(SetReadyTimerCo());
        PlayerManager.Instance.SetPlayerView(PlayerManager.Instance.Players[0].ID);
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClickCloseRoulette();
        }
    }
    
    private IEnumerator SetReadyTimerCo()
    {
        _readyTime = 20;
        while (true)
        {
            if (_readyTime <= 0)
            {
                _readyTime = 0;
                ReadyTimer.text = $"남은 시간: {_readyTime}";
                InGame.CurGameType = EGameType.Battle;
                break;
            }

            ReadyTimer.text = $"남은 시간: {Mathf.Ceil(_readyTime)}";
            _readyTime -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(0.1f);//아이템 드래그 취소 이후 딜레이
        PlayerManager.Instance.AutoSetItem();
        yield return new WaitForSeconds(1f);
        Debug.Log("전투 돌입");
        NetworkManager.Instance.SendBattleReadyPacket(PlayerManager.Instance.Players[0].ID, PlayerManager.Instance.Players[0].FirstAttack);
    }
    
    /// <summary>
    /// 룰렛창이 띄워진 상태에서 다른 공간을 클릭 했을때 룰렛창을 꺼줌
    /// </summary>
    private void ClickCloseRoulette()
    {
        if (RouletteWindow.activeSelf)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray2D ray = new Ray2D(pos, Vector2.zero);
            RaycastHit2D hit =
                Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, LayerMask.GetMask("Roulette"));

            if (hit.collider == null)
            {
                TryRoulette();
            }
        }
    }

    /// <summary>
    /// 룰렛 버튼의 활성화 여부
    /// </summary>
    public void TryInteractRoulette(int checkCount)
    {
        if (checkCount == Global.SlotMaxCount)
            RouletteButton.interactable = false;
        else
            RouletteButton.interactable = true;
    }
    
    public void TryRoulette()
    {
        _isActive = !_isActive;
        
        if(_isActive)
            RouletteWindow.SetActive(true);
        else
            RouletteWindow.SetActive(false);
    }

}
