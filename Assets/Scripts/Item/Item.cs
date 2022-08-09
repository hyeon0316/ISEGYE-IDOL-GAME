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
    private int _slotIndex1;
    private int _slotIndex2;

    public Vector3 OriginPos => _originPos;
    public Transform OriginParent => _originParent;

    public int SlotIndex1 => _slotIndex1;
    public int SlotIndex2 => _slotIndex2;


    protected string Name;

    [TextArea] protected string Description;

    public uint Upgrade = 0;

    [SerializeField] private int _code;

    public int Code
    {
        get { return _code; }
        set { _code = value; }
    }

    private Image _image;

    [SerializeField] private Sprite _sprite;

    public Sprite Sprite => _sprite;

    public virtual void Awake()
    {
        _image = GetComponent<Image>();
        _image.sprite = _sprite;
    }

    private void Update()
    {
        //전투단계로 전환될때 드래그 중이었던 아이템을 다시 원래 자리로 옮김
        if (this.transform.parent.name == "DragItem")
        {
            if (FindObjectOfType<Ready>().ReadyTime == 0)
            {
                transform.SetParent(_originParent);
                transform.position = _originPos;

                this.GetComponent<Image>().raycastTarget = true;
            }
        }
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
        if (InGame.CurGameType == GameType.Ready)
        {
            _originPos = this.transform.position;
            _originParent = this.transform.GetComponentInParent<ItemSlot>().transform;

            if (this.transform.parent.parent.TryGetComponent(out UsingInventory usingInventory))
                _slotIndex1 = this.transform.parent.GetSiblingIndex();
            else if (this.transform.parent.parent.TryGetComponent(out UnUsingInventory unUsingInventory))
                _slotIndex1 = this.transform.parent.GetSiblingIndex() + 6;

            transform.position = eventData.position;
            transform.SetParent(GameObject.Find("DragItem").transform);

            this.GetComponent<Image>().raycastTarget = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (InGame.CurGameType == GameType.Ready)
        {
            transform.position = new Vector3(eventData.position.x - Screen.width / 2,
                eventData.position.y - Screen.height / 2, 0);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (InGame.CurGameType == GameType.Ready)
        {
            ChangeSlot();
        }
    }

    private void ChangeSlot()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Ray2D ray = new Ray2D(pos, Vector2.zero);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, LayerMask.GetMask("ItemSlot"));

        if (hit.collider != null) //다른 슬롯에 드래그 했을 때
        {
            GameObject obj = hit.collider.GetComponent<ItemSlot>().gameObject;

            _slotIndex2 = obj.transform.GetSiblingIndex() +
                          (obj.transform.parent.name.Equals("UnUsingInventory") ? 6 : 0);
            PlayerManager playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
            Player player = playerManager.Players[playerManager.PlayerID - 1];
            if (player.UsingInventory.CheckDuplication(this))
            {
                RePosItem(_originParent, _originPos);
            }
            else
            {
                //player.SwapItem(_slotIndex1, _slotIndex2);
                NetworkManager.Instance.SendChangeItemSlotPacket(playerManager.Players[0].ID, (Int16) _slotIndex1,
                    (Int16) _slotIndex2);
            }
        }
        else //슬롯이 아닌 다른 공간에 드래그 했을 때
        {
            RePosItem(_originParent, _originPos);
        }

        this.GetComponent<Image>().raycastTarget = true;
    }


    public void RePosItem(Transform parent, Vector3 pos)
    {
        transform.SetParent(parent);
        transform.position = pos;
    }
}