using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class Ready : MonoBehaviour
{
    public GameObject RouletteWindow;
    
    public TextMeshProUGUI ReadyTimer;
    private float _readyTime;
    private bool _isActive = false;

    public float ReadyTime => _readyTime;
    
    private void OnEnable()
    {
        StartCoroutine(SetReadyTimerCo());
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (RouletteWindow.activeSelf)
            {
                ClickCloseRoulette();
            }
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
                InGame.CurGameType = GameType.Battle;
                break;
            }

            ReadyTimer.text = $"남은 시간: {Mathf.Ceil(_readyTime)}";
            _readyTime -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(1f);
        Debug.Log("전투 돌입");
        FindObjectOfType<InGame>().OpenBattle();        
    }
    
    /// <summary>
    /// 룰렛창이 띄워진 상태에서 다른 공간을 클릭 했을때 룰렛창을 꺼줌
    /// </summary>
    private void ClickCloseRoulette()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Ray2D ray = new Ray2D(pos, Vector2.zero);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, LayerMask.GetMask("Roulette"));

        if (hit.collider == null)
        {
            TryRoulette();
        }
    }

    public void TryRoulette()
    {
        _isActive = !_isActive;
        
        if(_isActive)
            OpenRoulette();
        else
            CloseRoulette();            
    }

    private void OpenRoulette()
    {
        RouletteWindow.SetActive(true);
    }
    
    private void CloseRoulette()
    {
        RouletteWindow.SetActive(false);
    }

}
