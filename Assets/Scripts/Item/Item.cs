using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Item : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Vector3 _originPos;
    private Transform _originParent;
    
    protected string Name;
    
    [TextArea]
    protected string Description;
    
    public uint Upgrade = 0;

    public uint Code { get; protected set; }
    
    private Image _image;
    
    [SerializeField]
    private Sprite _sprite;

    
    public virtual void Awake()
    {
        _image = GetComponent<Image>();
        _image.sprite = _sprite;
        _originPos = this.transform.position;
    }

    private void Start()
    {
        _originParent = this.transform.GetComponentInParent<ItemSlot>().transform;
    }

    public abstract void SetData();
        
    /// <summary>
    /// 아이템 효과 사용
    /// </summary>
    public abstract void Active();

    public virtual void ShowUsingEffect()
    {
        //todo: 아이템 사용 시 나오는 이펙트 효과 
    }
        
    public virtual void Show()
    {
        //todo: 아이템 첫 등장 시 나오는 이펙트 효과
    }

    public virtual void Hide()
    {
        //todo: 아이템 사용이후 사라질때 이펙트 효과
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        transform.SetParent(GameObject.Find("DragItem").transform);
        
        this.GetComponent<Image>().raycastTarget = false; //자기 자신 타겟 방지
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag 호출");

        ChangeSlot();

    }

    private void ChangeSlot()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Ray2D ray = new Ray2D(pos, Vector2.zero);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, LayerMask.GetMask("ItemSlot"));

        if (hit.collider != null)
        {
            Debug.Log("sdfsd");
            this.GetComponent<Image>().raycastTarget = true;
        }
        else
        {
            transform.SetParent(_originParent);
            transform.localPosition = _originPos;

            this.GetComponent<Image>().raycastTarget = true;
        }
        
    }

  
   
}
