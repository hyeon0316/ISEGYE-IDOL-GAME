using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InGame : MonoBehaviour
{
    public GameObject RouletteWindow;
    public GameObject ItemSelectWindow;
    public GameObject UpgradeWindow;
    
    private bool _isActive = false;
    private Image _itemImage;
    private TextMeshProUGUI _upgradeText;

    private void Awake()
    {
        //_itemImage = UpgradeWindow.transform.Find("ItemImage").GetComponent<Image>();
        //_upgradeText = UpgradeWindow.transform.Find("CurUpgrade").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        CloseRoulette();
        //CloseUpgrade();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (RouletteWindow.activeSelf)
            {
                ClickCloseRoulette();
            }
            ClickCancelUpgrade();
        }
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
    
    /// <summary>
    /// 업그레이드 모드에서 아이템 제외 다른 공간을 클릭 했을때 업그레이드 모드를 취소시켜줌
    /// </summary>
    private void ClickCancelUpgrade()
    {
        if (CursorManager.Instance.CurCursorType == CursorType.Upgrade)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Ray2D ray = new Ray2D(pos, Vector2.zero);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, LayerMask.GetMask("Item"));

            CursorManager.Instance.SetNomalCursor();
            
            if (hit.collider == null)
            {
                Debug.Log("강화 취소");
            }
            else if (hit.collider != null)
            {
                Debug.Log("강화 시도");
                Item item = hit.collider.GetComponent<Item>();
                
                OpenUpgrade();
                _itemImage.sprite = item.Sprite;
                _upgradeText.text = $"업그레이드: {item.Upgrade}";
            }
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

    public void OpenItemSelect()
    {
        ItemSelectWindow.SetActive(true);    
    }
    
    public void CloseItemSelect()
    {
        ItemSelectWindow.SetActive(false);    
    }
    
    private void OpenRoulette()
    {
        RouletteWindow.SetActive(true);
    }
    
    private void CloseRoulette()
    {
        RouletteWindow.SetActive(false);
    }

    public void OpenUpgrade()
    {
        UpgradeWindow.SetActive(true);
    } 
    
    public void CloseUpgrade()
    {
        UpgradeWindow.SetActive(false);
    }
    
}
